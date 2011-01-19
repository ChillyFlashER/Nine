#region Copyright 2009 - 2010 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2010 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Nine;
using Nine.Graphics;
#if !WINDOWS_PHONE
using Nine.Graphics.Effects;
#endif
using Nine.Animations;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// Sample game showing how to display skinned character animation.
    /// </summary>
    public class SkinnedModelGame : Microsoft.Xna.Framework.Game
    {
#if WINDOWS_PHONE
        private const int TargetFrameRate = 30;
        private const int BackBufferWidth = 800;
        private const int BackBufferHeight = 480;
#elif XBOX
        private const int TargetFrameRate = 60;
        private const int BackBufferWidth = 1280;
        private const int BackBufferHeight = 720;
#else
        private const int TargetFrameRate = 60;
        private const int BackBufferWidth = 900;
        private const int BackBufferHeight = 600;
#endif

        Model model;
        Model hammer;
        ModelBatch modelBatch;
        PrimitiveBatch primitiveBatch;
        Input input;
        Random random = new Random();
        ModelViewerCamera camera;

        LookAtController lookAtController;

        BoneAnimation animation1;
        BoneAnimation animation2;
        BoneAnimation animation3;

        Matrix world1 = Matrix.CreateTranslation(-80, -60, 0) * Matrix.CreateScale(0.1f);
        Matrix world2 = Matrix.CreateTranslation(80, -60, 0) * Matrix.CreateScale(0.1f);
        Matrix world3 = Matrix.CreateTranslation(0, -60, 0) * Matrix.CreateScale(0.1f);

        public SkinnedModelGame()
        {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);

            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = BackBufferWidth;
            graphics.PreferredBackBufferHeight = BackBufferHeight;

            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / TargetFrameRate);
            
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = false;
            Components.Add(new FrameRate(this, "Consolas"));
            Components.Add(new InputComponent(Window.Handle));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a model viewer camera to help us visualize the scene
            camera = new ModelViewerCamera(GraphicsDevice);

            // Model batch makes drawing models easier
            modelBatch = new ModelBatch(GraphicsDevice);
            primitiveBatch = new PrimitiveBatch(GraphicsDevice);

            // Load our model assert.
            // If the model is processed by our ExtendedModelProcesser,
            // we will try to add model animation and skinning data.
            model = Content.Load<Model>("peon");
            hammer = Content.Load<Model>("hammer");

#if WINDOWS_PHONE
            // Convert the effect used by the model to SkinnedEffect to
            // support skeleton animation.
            SkinnedEffect skinned = new SkinnedEffect(GraphicsDevice);
            skinned.EnableDefaultLighting();

            // ModelBatch will use this skinned effect to draw the model.
            model.ConvertEffectTo(skinned);
#else
            LinkedEffect linkedEffect = Content.Load<LinkedEffect>("SkinnedEffect");
            linkedEffect.EnableDefaultLighting();
            model.ConvertEffectTo(linkedEffect);
#endif       

            // Handle animations.
            animation1 = PlayAttackAndRun();
            animation2 = PlayIdleRunCarryBlended();
            animation3 = PlayAnimation(0);

            input = new Input();
            input.MouseDown += new EventHandler<MouseEventArgs>(input_MouseDown);
        }

        private void input_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                animation3 = PlayAnimation(random.Next(model.GetAnimations().Count));
            }
        }

        private BoneAnimation PlayAnimation(int i)
        {   
            // Now load our model animation and skinning using extension method.
            BoneAnimationController animation = new BoneAnimationController(model.GetAnimation(i));
            //animation.Speed = 0.04f;
            //animation.Ending = KeyframeEnding.Clamp;
            //animation.InterpolationEnabled = true;
            //animation.Repeat = 1.5f;
            //animation.AutoReverse = true;
            //animation.StartupDirection = AnimationDirection.Backward;

            // You must specify a unique value for each mesh instance to enabled default blending.
            // Do it by pass the unique object into the second parameter of BoneAnimation constructor.
            BoneAnimation result = new BoneAnimation(model, this);
            result.Controllers.Add(animation);
            result.BlendDuration = TimeSpan.FromSeconds(1f);
            result.Play();
            return result;
        }

        private BoneAnimation PlayAttackAndRun()
        {
            BoneAnimationController attack = new BoneAnimationController(model.GetAnimation("Attack"));
            BoneAnimationController run = new BoneAnimationController(model.GetAnimation("Run"));
            run.Speed = 0.8f;

            BoneAnimation blended = new BoneAnimation(model, null);
            blended.Controllers.Add(run);
            blended.Controllers.Add(attack);
            
            blended.Controllers[run].Disable("Bip01_Pelvis", false);
            blended.Controllers[run].Disable("Bip01_Spine1", true);

            blended.Controllers[attack].Disable("Bip01", false);
            blended.Controllers[attack].Disable("Bip01_Spine", false);
            blended.Controllers[attack].Disable("Bip01_L_Thigh", true);
            blended.Controllers[attack].Disable("Bip01_R_Thigh", true);

            blended.KeyController = run;
            blended.IsSychronized = true;
            blended.Play();

            return blended;
        }

        private BoneAnimation PlayIdleRunCarryBlended()
        {
            // Blend between 3 animation channels
            BoneAnimationController idle = new BoneAnimationController(model.GetAnimation("Idle"));
            BoneAnimationController carry = new BoneAnimationController(model.GetAnimation("Carry"));
            BoneAnimationController run = new BoneAnimationController(model.GetAnimation("Run"));

            BoneAnimation blended = new BoneAnimation(model, null);
            blended.Controllers.Add(idle);
            blended.Controllers.Add(run);
            blended.Controllers.Add(carry);

            lookAtController = new LookAtController(blended, world2, model.Bones["Bip01_Head"].Index);
            lookAtController.Up = Vector3.UnitX;
            lookAtController.Forward = -Vector3.UnitZ;
            lookAtController.HorizontalRotation = new Range<float>(-MathHelper.PiOver2, MathHelper.PiOver2);
            lookAtController.VerticalRotation = new Range<float>(-MathHelper.PiOver2, MathHelper.PiOver2);
            blended.Controllers.Add(lookAtController);

            // Give the look at controller a huge blend weight so it will dominate the other controllers.
            // All the weights will be normalized during blending.
            blended.Controllers[lookAtController].BlendWeight = 10;

            blended.KeyController = run;
            blended.IsSychronized = true;
            blended.Play();

            return blended;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Makes the model looks at the camera
            lookAtController.Target = Matrix.Invert(camera.View).Translation;

            animation1.Update(gameTime);
            animation2.Update(gameTime);
            animation3.Update(gameTime);

            GraphicsDevice.Clear(Color.DarkSlateGray);

            // Attach the hammer model to the character
            Matrix hammerTransform = animation1.GetAbsoluteBoneTransform("Weapon") * world1;

            if (!Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                modelBatch.Begin(ModelSortMode.Deferred, camera.View, camera.Projection);
                {
                    modelBatch.DrawSkinned(model, world1, animation1.GetBoneTransforms(), null);
                    modelBatch.DrawSkinned(model, world2, animation2.GetBoneTransforms(), null);
                    modelBatch.DrawSkinned(model, world3, animation3.GetBoneTransforms(), null);

                    modelBatch.Draw(hammer, hammerTransform, null);
                }
                modelBatch.End();
            }
            else
            {
                primitiveBatch.Begin(camera.View, camera.Projection);
                {
                    primitiveBatch.DrawSkeleton(model, world3, Color.Yellow);

                    primitiveBatch.DrawSkeleton(animation1, world1, Color.White);
                    primitiveBatch.DrawSkeleton(animation2, world2, Color.White);
                    primitiveBatch.DrawSkeleton(animation3, world3, Color.White);

                    primitiveBatch.DrawAxis(animation2.GetAbsoluteBoneTransform("Bip01_Head") * world2, 1);
                }
                primitiveBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
