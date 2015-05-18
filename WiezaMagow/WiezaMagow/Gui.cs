using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TowerDef
{
    public class Gui:GameObject
    {
        private class GuiImage
        {
            public Texture2D def;
            public Texture2D selected;
            public bool isSelected;

            private GuiImage() { }
            public GuiImage(Texture2D def, Texture2D selected)
            {
                this.def = def;
                this.selected = selected;
            }

        }

        Texture2D rightBar, texTower1, texTower2, texTower3, texTower4, texTowerSelected1, texTowerSelected2, texTowerSelected3, texTowerSelected4;

        GuiImage tower1, tower2, tower3, tower4;

        SpriteBatch spriteBatch;

        Texture2D selected;

        public Gui(SpriteBatch spriteBatch){
            this.spriteBatch = spriteBatch;
        }

        private Gui()
        {
        }

        public override void loadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {

            tower1 = new GuiImage(
                content.Load<Texture2D>("Textures\\GUI\\towerImage1"),
                content.Load<Texture2D>("Textures\\GUI\\towerImage1-selected")
            );

            tower2 = new GuiImage(
                content.Load<Texture2D>("Textures\\GUI\\towerImage2"),
                content.Load<Texture2D>("Textures\\GUI\\towerImage2-selected")
            );

            tower3 = new GuiImage(
                content.Load<Texture2D>("Textures\\GUI\\towerImage3"),
                content.Load<Texture2D>("Textures\\GUI\\towerImage3-selected")
            );

            tower4 = new GuiImage(
                content.Load<Texture2D>("Textures\\GUI\\towerImage4"),
                content.Load<Texture2D>("Textures\\GUI\\towerImage4-selected")
            );

            rightBar = content.Load<Texture2D>("Textures\\GUI\\right-bar");

            tower1.isSelected = true;
        }

        public override void draw()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(rightBar, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(tower1.isSelected ? tower1.selected : tower1.def, new Vector2(5, 10), Color.White);
            spriteBatch.Draw(tower2.isSelected ? tower2.selected : tower2.def, new Vector2(5, 110), Color.White);
            spriteBatch.Draw(tower3.isSelected ? tower3.selected : tower3.def, new Vector2(5, 210), Color.White);
            spriteBatch.Draw(tower4.isSelected ? tower4.selected : tower4.def, new Vector2(5, 310), Color.White);
            spriteBatch.End();
        }

        public void select(int i)
        {
            switch (i)
            {
                case 1:
                    clearSelection();
                    tower1.isSelected = true;
                    break;
                case 2:
                    clearSelection();
                    tower2.isSelected = true;
                    break;
                case 3:
                    clearSelection();
                    tower3.isSelected = true;
                    break;
                case 4:
                    clearSelection();
                    tower4.isSelected = true;
                    break;
            }
        }

        private void clearSelection()
        {
            tower1.isSelected = false;
            tower2.isSelected = false;
            tower3.isSelected = false;
            tower4.isSelected = false;
        }

        public override void update()
        {
        }
    }
}
