using Engine;
using Engine.Engines;
using GameEngineProgramming.Engine.Engines;
using Microsoft.Xna.Framework;


namespace GameEngineProgramming.Engine.Base
{
    public class GameEngine : DrawableGameComponent
    {
        private InputEngine input;
        private FrameRateCounter FPScounter { get; set; }
        public CameraEngine Cameras { get; set; }
        public DebugEngine Debug;
        public PhysicsEngine Physics { get; set; }
        private static Scene activeScene;
        public static Scene ActiveScene { get { return activeScene; } }

        //Manages sub-engines and the scene
        public GameEngine(Game gameInstance) :
            base(gameInstance)
        {
            Game.Components.Add(this);
            input = new InputEngine(Game);
            Cameras = new CameraEngine(Game);
            Physics = new PhysicsEngine(Game);
            FPScounter = new FrameRateCounter(Game);
            Debug = new DebugEngine();
        }

        public override void Initialize()
        {
            Debug = new DebugEngine();
            Debug.Initialize();
            base.Initialize();
        }

        public void LoadScene(Scene newScene)
        {
            if(newScene != null)
            {
                if (activeScene != null)
                {
                    UnloadScene();
                }
                //continue to load new scene
                activeScene = newScene;
                activeScene.Initialize();
            }
        }//end of LoadScene

        public void UnloadScene()
        {
            if (activeScene != null)
            {
                activeScene = null;
                Cameras.Clear();//removes cameras
                GameUtilities.UnloadNonPersistentContent();//clear content
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (activeScene != null)
                activeScene.Update();

            base.Update(gameTime);
        }

        public void Draw3D(CameraComponent camera)
        {
            activeScene.Draw3D(camera);
            Debug.Draw(camera);
        }

        public void Draw2D()
        {
            activeScene.DrawUI();
        }

        public override void Draw(GameTime gameTime)
        {
            if (activeScene != null)
            {
                if (CameraEngine.ActiveCamera != null)
                    Draw3D(CameraEngine.ActiveCamera);

                Draw2D();
                GameUtilities.SetGraphicsDeviceFor3D();
            }
            base.Draw(gameTime);
        }
    }
}