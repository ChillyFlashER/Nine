//-----------------------------------------------------------------------------
//  Isles v1.0
//  
//  Copyright 2008 (c) Nightin Games. All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Isles.Engine;
using Isles.Graphics;

namespace Isles.Engine
{
    /// <summary>
    /// Represents the game world
    /// </summary>
    public class GameWorld : ISceneManager
    {        
        #region Field
        /// <summary>
        /// Version of the game world
        /// </summary>
        public const int Version = 1;

        protected sealed class InternalList<T> : BroadcastList<T, LinkedList<T>> {}

        /// <summary>
        /// Enumerates all world objects
        /// </summary>
        public IEnumerable<IWorldObject> WorldObjects
        {
            get { return worldObjects; }
        }

        protected InternalList<IWorldObject> worldObjects = new InternalList<IWorldObject>();


        /// <summary>
        /// Gets or sets the texture used to draw the selection
        /// </summary>
        public Texture2D SelectionTexture;


        /// <summary>
        /// Gets all selected entities
        /// </summary>
        public List<Entity> Selected
        {
            get { return selected; }
        }

        protected List<Entity> selected = new List<Entity>();


        /// <summary>
        /// Gets all highlighted entites
        /// </summary>
        public IEnumerable<Entity> Highlighted
        {
            get { return highlighted; }
        }

        protected List<Entity> highlighted = new List<Entity>();

        
        /// <summary>
        /// Landscape of the game world
        /// </summary>
        public Landscape Landscape
        {
            get { return landscape; }
        }

        protected Landscape landscape;
        protected string landscapeFilename;


        /// <summary>
        /// Game content manager.
        /// Assets loaded using this content manager will not be unloaded
        /// until the termination of the application.
        /// </summary>
        public ContentManager Content
        {
            get { return content; }
        }

        protected ContentManager content;


        /// <summary>
        /// Content manager for a single level/world.
        /// Assets loaded using this content manager is unloaded each time
        /// a game world is released.
        /// </summary>
        public ContentManager LevelContent
        {
            get { return levelContent; }
        }

        protected ContentManager levelContent;


        /// <summary>
        /// Gets game logic
        /// </summary>
        public GameLogic GameLogic
        {
            get { return gameLogic; }
        }

        protected GameLogic gameLogic = new GameLogic();


        /// <summary>
        /// CUrrent level
        /// </summary>
        public ILevel Level
        {
            get { return level; }
        }

        protected ILevel level;
        protected string levelName;


        /// <summary>
        /// Game UI, can be null
        /// </summary>
        public IGameUI UI
        {
            get { return ui; }
            set { ui = value; }
        }

        protected IGameUI ui;

        public string Name;
        public string Description;
        #endregion

        #region Methods
        public GameWorld()
        {
            this.content = BaseGame.Singleton.Content;
            this.levelContent = new ContentManager(BaseGame.Singleton.Services);
            this.levelContent.RootDirectory = content.RootDirectory;
        }

        /// <summary>
        /// Reset the game world
        /// </summary>
        public void Reset()
        {
            gameLogic.Reset();
        }

        /// <summary>
        /// Update the game world and all the world objects
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Set picked entity to null
            pickedEntity = null;

            // Update internal lists
            worldObjects.Update();

            // Update landscape
            landscape.Update(gameTime);

            // Update each object
            foreach (IWorldObject o in worldObjects)
                o.Update(gameTime);

            // Update level
            if (level != null)
                level.Update(gameTime);

            // Update scene manager internal structure
            UpdateSceneManager();
        }

        /// <summary>
        /// Draw all world objects
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            landscape.PreDraw(gameTime);
            landscape.Draw(gameTime);

            foreach (IWorldObject o in worldObjects)
                o.Draw(gameTime);

            DrawSelection(gameTime);

            if (level != null)
                level.Draw(gameTime);
        }

        private void DrawSelection(GameTime gameTime)
        {
            if (SelectionTexture == null)
                return;

            foreach (Entity e in selected)
            {
                if (e != null)
                {
                    Vector3 size = e.Size;
                    float radius = Math.Max(size.X, size.Y) * 1.5f;

                    landscape.DrawSurface(
                        SelectionTexture,
                        new Vector2(e.Position.X, e.Position.Y),
                        new Vector2(radius, radius));
                }
            }
        }

        /// <summary>
        /// Each level has a unique name stored in this dictionary
        /// </summary>
        static Dictionary<string, ILevel> levelDictionary = new Dictionary<string, ILevel>();

        /// <summary>
        /// Register a new level logic
        /// </summary>
        /// <param name="levelName"></param>
        /// <param name="level"></param>
        public static void RegisterLevel(string levelName, ILevel level)
        {
            levelDictionary.Add(levelName, level);
        }

        /// <summary>
        /// Load the game world from a file
        /// </summary>
        /// <param name="inStream"></param>
        public virtual void Load(XmlElement node, ILoading context)
        {
            // Validate XML element
            if (node.Name != "World")
                throw new Exception("Invalid world format.");

            // Validate version
            int version = -1;
            if (!(int.TryParse(node.GetAttribute("Version"), out version) && version == Version))
                throw new Exception("Invalid world version");

            // Load landscape
            landscapeFilename = node.GetAttribute("Landscape");
            if (landscapeFilename == "")
                throw new Exception("World does not have a landscape");

            landscape = levelContent.Load<Landscape>(landscapeFilename);
            
            // Name & description
            Name = node.GetAttribute("Name");
            Description = node.GetAttribute("Description");

            context.Refresh(10);

            // Load world objects
            int nObjects = 0;
            IWorldObject worldObject;
            foreach (XmlNode child in node.ChildNodes)
            {
                // Ignore comments and other stuff...
                XmlElement element = (child as XmlElement);

                if (element != null)
                {
                    if ((worldObject = Create(child.Name, element)) != null)
                        nObjects++;
                }

                context.Refresh(10 + (int)(80 * nObjects / node.ChildNodes.Count));
            }

            context.Refresh(90);

            // Find ILevel from level attribute
            levelName = node.GetAttribute("Level");

            if (levelDictionary.ContainsKey(levelName))
            {
                level = levelDictionary[levelName];

                // Load new level
                level.Load(this, context);
            }

            Log.Write("Game world loaded [" + Name + "], " + nObjects + " objects...");
        }

        /// <summary>
        /// Save the world to a file
        /// </summary>
        /// <param name="outStream"></param>
        public virtual void Save(XmlElement node, ILoading context)
        {
            XmlElement child;
            XmlElement header;
            XmlDocument doc = node.OwnerDocument;

            // Create a default comment
            node.AppendChild(doc.CreateComment(
                "Isles.Engine Generated World: " + DateTime.Now.ToString()));

            // Append a new element as the root node of the world
            node.AppendChild(header = doc.CreateElement("World"));

            // Setup attributes
            header.SetAttribute("Version", Version.ToString());
            header.SetAttribute("Name", Name);
            header.SetAttribute("Description", Description);
            header.SetAttribute("Landscape", landscapeFilename);

            if (level != null && levelName != null)
                header.SetAttribute("Level", levelName);

            // Serialize world objects
            int nObjects = 0;
            foreach (IWorldObject worldObject in worldObjects)
            {
                if (worldObject.ClassID != null &&
                   (child = doc.CreateElement(worldObject.ClassID)) != null)
                {
                    header.AppendChild(child);
                    worldObject.Serialize(child);
                    nObjects++;
                }
            }

            Log.Write("Game world saved [" + Name + "], " + nObjects + " objects...");
        }

        /// <summary>
        /// Delegate to realize factory method
        /// </summary>
        public delegate IWorldObject Creator(GameWorld world);

        /// <summary>
        /// This dictionary holds all the info to create a world object of a given type.
        /// For a given type of object, the create funtion calls its corresponding Creator,
        /// which is responsible for performing the actual creation stuff.
        /// 
        /// I haven't figure out a better way to do this.
        /// If you know how, let me know it ASAP :)
        /// </summary>
        static Dictionary<string, Creator> creators = new Dictionary<string, Creator>();

        /// <summary>
        /// Register a world object creator.
        /// If a new type of world object is implemented, to allow creating the object using
        /// GameWorld.Create, create an object creator and register it here.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="creator"></param>
        public static void RegisterCreator(Type type, Creator creator)
        {
            creators.Add(type.Name, creator);
        }

        /// <summary>
        /// Register a world object creator
        /// </summary>
        public static void RegisterCreator(string typeName, Creator creator)
        {
            creators.Add(typeName, creator);
        }

        /// <summary>
        /// Create a new world object of a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>null if the type is not supported</returns>
        public IWorldObject Create(Type type)
        {
            return Create(type.Name, null);
        }

        /// <summary>
        /// Creates a new world object from a given type
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public IWorldObject Create(string typeName)
        {
            return Create(typeName, null);
        }

        public IWorldObject Create(Type type, XmlElement xml)
        {
            return Create(type.Name, xml);
        }

        /// <summary>
        /// Creates a new world object of a given type
        /// </summary>
        /// <param name="typeName">Type of the object</param>
        /// <param name="xml">A xml element describes the object</param>
        /// <returns></returns>
        public IWorldObject Create(string typeName, XmlElement xml)
        {
            // Lookup the creators table to find a suitable creator
            if (!creators.ContainsKey(typeName))
                throw new Exception("Unknown object type: " + typeName);

            // Delegate to the creator
            IWorldObject worldObject = creators[typeName](this);

            // Nothing created
            if (worldObject == null)
                return null;

            // Set object class ID
            worldObject.ClassID = typeName;

            // Deserialize world object
            if (xml != null)
                worldObject.Deserialize(xml);
            
            // Add the new object to the world
            Add(worldObject);

            return worldObject;
        }

        /// <summary>
        /// Select a world object, pass null to deselect everything
        /// </summary>
        /// <param name="select"></param>
        public void Select(Entity obj)
        {
            foreach (Entity e in selected)
                e.Selected = false;

            selected.Clear();

            if (obj != null)
            {
                obj.Selected = true;
                selected.Add(obj);
            }

            // Refact the selection event to UI
            if (ui != null)
                ui.Select(obj);
        }

        /// <summary>
        /// Select multiple entites
        /// </summary>
        /// <param name="objects"></param>
        public void SelectMultiple(IEnumerable<Entity> objects)
        {
            foreach (Entity e in selected)
                e.Selected = false;

            selected.Clear();

            selected.AddRange(objects);

            foreach (Entity e in selected)
                e.Selected = true;
            
            // Refact the selection event to UI
            if (ui != null)
                ui.SelectMultiple(objects);
        }

        /// <summary>
        /// Highlight a world object, pass null to dehighlight everything
        /// </summary>
        /// <param name="obj"></param>
        public void Highlight(Entity obj)
        {
            foreach (Entity e in highlighted)
                e.Highlighted = false;

            highlighted.Clear();

            if (obj != null)
            {
                highlighted.Add(obj);
                obj.Highlighted = true;
            }
        }

        /// <summary>
        /// Highlight multiple entities
        /// </summary>
        /// <param name="objects"></param>
        public void HighlightMultiple(IEnumerable<Entity> objects)
        {
            foreach (Entity e in highlighted)
                e.Highlighted = false;

            highlighted.Clear();

            highlighted.AddRange(objects);

            foreach (Entity e in highlighted)
                e.Highlighted = true;
        }
        #endregion

        #region Pick
        /// <summary>
        /// Entity picked this frame
        /// </summary>
        Entity pickedEntity;

        /// <summary>
        /// Pick an entity from the cursor
        /// </summary>
        /// <returns></returns>
        public Entity Pick()
        {
            if (pickedEntity != null)
                return pickedEntity;

            // Cache the result
            return pickedEntity = Pick(BaseGame.Singleton.PickRay);
        }

        /// <summary>
        /// Pick grid offset
        /// </summary>
        readonly Point[] PickGridOffset = new Point[9]
        {
            new Point(-1, -1), new Point(0, -1), new Point(1, -1),
            new Point(-1, 0) , new Point(0, 0) , new Point(1, 0) ,
            new Point(-1, 1) , new Point(0, 1) , new Point(1, 1) ,
        };

        /// <summary>
        /// Pick a game entity from the given gay
        /// </summary>
        /// <returns></returns>
        public Entity Pick(Ray ray)
        {
            // This value affects how accurate this algorithm works.
            // Basically, a sample point starts at the origion of the
            // pick ray, it's position incremented along the direction
            // of the ray each step with a value of PickPrecision.
            // A pick precision of half the grid size is good.
            const float PickPrecision = 5.0f;

            // This is the bounding box for all game entities
            BoundingBox boundingBox = landscape.TerrainBoundingBox;
            boundingBox.Max.Z += Entity.MaxHeight;

            // Nothing will be picked if the ray doesn't even intersects
            // with the bounding box of all grids
            Nullable<float> result = ray.Intersects(boundingBox);
            if (!result.HasValue)
                return null;

            // Initialize the sample point
            Vector3 step = ray.Direction * PickPrecision;
            Vector3 sampler = ray.Position + ray.Direction * result.Value;

            // Keep track of the grid visited previously, so that we can
            // avoid checking the same grid.
            Point previousGrid = new Point(-1, -1);

            while ( // Stop probing if we're outside the box
                boundingBox.Contains(sampler) == ContainmentType.Contains)
            {
                // Project to XY plane and get which grid we're in
                Point grid = landscape.PositionToGrid(sampler.X, sampler.Y);

                // If we hit the ground, nothing is picked
                if (landscape.HeightField[grid.X, grid.Y] > sampler.Z)
                    return null;

                // Check the grid visited previously
                if (grid.X != previousGrid.X || grid.Y != previousGrid.Y)
                {
                    // Check the 9 adjacent grids in case we miss the some
                    // entities like trees (Trees are big at the top but are
                    // small at the bottom).
                    // Also find the minimum distance from the entity to the
                    // pick ray position to make the pick correct

                    Point pt;
                    float shortest = 10000;
                    Entity pickEntity = null;

                    for (int i = 0; i < PickGridOffset.Length; i++)
                    {
                        pt.X = grid.X + PickGridOffset[i].X;
                        pt.Y = grid.Y + PickGridOffset[i].Y;

                        if (landscape.IsValidGrid(pt))
                        {
                            foreach (Entity entity in landscape.Data[pt.X, pt.Y].Owners)
                            {
                                Nullable<float> value = entity.Intersects(ray);

                                if (value.HasValue && value.Value < shortest)
                                {
                                    shortest = value.Value;
                                    pickEntity = entity;
                                }
                            }
                        }
                    }

                    if (pickEntity != null)
                        return pickEntity;

                    previousGrid = grid;
                }

                // Sample next position
                sampler += step;
            }

            return null;
        }
        #endregion

        #region ISceneManager Members

        /// <summary>
        /// Adds a new world object
        /// </summary>
        public void Add(IWorldObject worldObject)
        {
            worldObjects.Add(worldObject);
        }

        /// <summary>
        /// Destroy a scene object
        /// </summary>
        /// <param name="worldObject"></param>
        public void Destroy(IWorldObject worldObject)
        {
            // Deactivate the object
            if (worldObject.IsActive)
                Deactivate(worldObject);

            // Remove it from selected and highlighed
            Entity e = worldObject as Entity;

            if (e != null)
            {
                if (selected.Contains(e))
                    selected.Remove(e);

                if (highlighted.Contains(e))
                    highlighted.Remove(e);
            }

            // Finally, remove it from object list
            worldObjects.Remove(worldObject);
        }

        public void Clear()
        {
            worldObjects.Clear();
        }

        /// <summary>
        /// Activate a world object
        /// </summary>
        public void Activate(IWorldObject worldObject)
        {
            if (worldObject == null)
                throw new ArgumentNullException();

            if (worldObject.IsActive)
                return;

            worldObject.IsActive = true;

            foreach (Point grid in landscape.EnumerateGrid(worldObject.BoundingBox))
            {
                System.Diagnostics.Debug.Assert(
                    !landscape.Data[grid.X, grid.Y].Owners.Contains(worldObject));

                landscape.Data[grid.X, grid.Y].Owners.Add(worldObject);
            }

            worldObject.DirtyBoundingBox = worldObject.BoundingBox;
            worldObject.IsDirty = false;
        }

        /// <summary>
        /// Deactivate a world object
        /// </summary>
        public void Deactivate(IWorldObject worldObject)
        {
            if (worldObject == null)
                throw new ArgumentNullException();

            if (!worldObject.IsActive)
                return;

            worldObject.IsActive = false;

            foreach (Point grid in landscape.EnumerateGrid(
                worldObject.IsDirty ? worldObject.DirtyBoundingBox : worldObject.BoundingBox))
            {
                System.Diagnostics.Debug.Assert(
                    landscape.Data[grid.X, grid.Y].Owners.Contains(worldObject));

                landscape.Data[grid.X, grid.Y].Owners.Remove(worldObject);
            }

            worldObject.IsDirty = false;
        }
        
        void UpdateSceneManager()
        {
            // For all active objects, change the grids it owns if its
            // bounding box is dirty, making it up to date.
            foreach (IWorldObject o in worldObjects)
            {
                if (o.IsActive && o.IsDirty)
                {
                    // Remove old grids
                    foreach (Point grid in
                        landscape.EnumerateGrid(o.DirtyBoundingBox))
                    {
                        System.Diagnostics.Debug.Assert(
                            landscape.Data[grid.X, grid.Y].Owners.Contains(o));

                        landscape.Data[grid.X, grid.Y].Owners.Remove(o);
                    }

                    // Add to new grids
                    foreach (Point grid in
                        landscape.EnumerateGrid(o.BoundingBox))
                    {
                        System.Diagnostics.Debug.Assert(
                           !landscape.Data[grid.X, grid.Y].Owners.Contains(o));

                        landscape.Data[grid.X, grid.Y].Owners.Add(o);
                    }

                    // Change dirty bounding box
                    o.DirtyBoundingBox = o.BoundingBox;

                    // Make the dirty bounding box clean
                    o.IsDirty = false;
                }
            }
        }

        /// <summary>
        /// Test to see if a point intersects a world object
        /// </summary>
        public bool PointSceneIntersects(Vector3 point)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test to see if a ray intersects a world object
        /// </summary>
        public bool RaySceneIntersects(Ray ray)
        {
            throw new NotImplementedException();
        }

        public bool ObjectIntersects(IWorldObject object1, IWorldObject object2)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IWorldObject> ObjectsFromPoint(Vector3 point)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEnumerable<IWorldObject> ObjectsFromRay(Ray ray)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEnumerable<IWorldObject> ObjectsFromRegion(BoundingBox boundingBox)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEnumerable<IWorldObject> ObjectsFromRegion(BoundingFrustum boundingFrustum)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEnumerable<IWorldObject> GetNearbyObjects(Vector3 position)
        {
            Point grid = landscape.PositionToGrid(position.X, position.Y);

            return landscape.Data[grid.X, grid.Y].Owners;
        }

        public IEnumerable<IWorldObject> GetNearbyObjects(BoundingBox volume)
        {
            List<IWorldObject> set = new List<IWorldObject>(4);

            foreach (Point grid in landscape.EnumerateGrid(volume))
            {
                foreach (IWorldObject o in landscape.Data[grid.X, grid.Y].Owners)
                    if (!set.Contains(o))
                        set.Add(o);
            }

            return set;
        }

        public IWorldObject ObjectFromName(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
