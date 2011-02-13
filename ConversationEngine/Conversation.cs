using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ConversationEngine
{
    //---------------------------------------------------------------------------------------
    //
    // Conversation
    //  TODO: Many Comments
    //
    //---------------------------------------------------------------------------------------
    public class Conversation : DrawableGameComponent
    {
        private List<string> Keybinds = new List<string>()
        {
            "A",
            "S",
            "D",
            "F"
        };

        SpriteBatch spriteBatch;
        Rectangle screenBounds;
        SpriteFont font;
        DialogTree dialogTree;
        DialogNode currentNode;
        DialogNode nextNode;
        double timeSinceNodeSelect;

        public Conversation(Game game, ref InputManager inputManager, SpriteBatch spriteBatch)
            : base(game)
        {
            game.Components.Add(this);

            screenBounds = game.GraphicsDevice.Viewport.Bounds;
            
            inputManager.KB_Any += new InputManager.KBEvent(OnKB);

            this.spriteBatch = spriteBatch;

            font = game.Content.Load<SpriteFont>("Segoe");

            dialogTree = new DialogTree("data/CharlieTree.xml");
            currentNode = dialogTree.GetFirstNode();

            timeSinceNodeSelect = 0.0;
        }

        private void OnKB(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.A))
                SelectResponse("A");
            else if (state.IsKeyDown(Keys.S))
                SelectResponse("S");
            else if (state.IsKeyDown(Keys.D))
                SelectResponse("D");
            else if (state.IsKeyDown(Keys.F))
                SelectResponse("F");
        }

        private void SelectResponse(string keyPressed)
        {
            if (currentNode == null || currentNode.responses == null) return;

            int responseSelected = -1;
            for(int i = 0; i < Keybinds.Count; i++)
            {
                if (Keybinds[i] == keyPressed)
                    responseSelected = i;
            }
            if (responseSelected == -1 || responseSelected >= currentNode.responses.Count) return;

            nextNode = dialogTree.GetNodeByName(currentNode.responses[responseSelected].nextNode);
            currentNode = null;
        }

        public override void Update(GameTime gameTime)
        {
            if (nextNode != null)
            {
                timeSinceNodeSelect += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeSinceNodeSelect > 0.3)
                {
                    currentNode = nextNode;
                    currentNode.NextText();
                    DoDialogAction();
                    nextNode = null;
                    timeSinceNodeSelect = 0.0;
                }
            }
        }

        private void DoDialogAction()
        {
            if (currentNode.action == null) return;

            string action = currentNode.action.name;
            List<string> actionParams = currentNode.action.strParams;

            if (action == "music")
            {
                Console.WriteLine("playing music: " + actionParams[0]);
            }
            else if (action == "sfx")
            {
                Console.WriteLine("playing sfx: " + actionParams[0]);
            }
            else
            {
                Console.WriteLine("action not recognized: " + action + " param1: " + actionParams[0]);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            DrawDialogNode();
            spriteBatch.DrawString(font, "[Esc] to Exit", new Vector2(0f, screenBounds.Height - font.LineSpacing), Color.White);
            spriteBatch.End();
        }

        private void DrawDialogNode()
        {
            if (currentNode == null) return;

            string nodeText = WrapText(font, currentNode.GetCurrentText(), screenBounds.Width);
            spriteBatch.DrawString(font, nodeText, Vector2.Zero, Color.White);

            DrawResponses();
        }

        private void DrawResponses()
        {
            if (currentNode.responses == null) return;

            Vector2 startPos = new Vector2(100f, 100f);
            int lineHeight = font.LineSpacing;
            int lineNum = 0;
            int extraLines = 0;
            foreach (Response response in currentNode.responses)
            {
                Vector2 linePos = new Vector2(startPos.X, startPos.Y + ((lineNum + extraLines) * lineHeight));
                string fmtText = "(" + Keybinds[lineNum] + ") " + response.text;
                fmtText = WrapText(font, fmtText, screenBounds.Width - linePos.X);
                // increment lineNum for each carriage return
                int crCount = fmtText.Split('\n').Length - 1;
                extraLines = crCount;
                spriteBatch.DrawString(font, fmtText, linePos, Color.White);
                lineNum++;
            }
        }

        public string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }
    }
}
