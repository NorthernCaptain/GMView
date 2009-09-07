using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;

namespace GMView
{
    public class Bookmark: ISprite
    {
        [XmlIgnore]
        public string id;
        [XmlAttribute]
        public double lon, lat;
        [XmlAttribute]
        public string name;
        [XmlElement]
        public string comment;
        [XmlAttribute]
        public int image_idx = 0;

        [XmlIgnore]
        public bool shown = false;
        [XmlAttribute]
        public MapTileType original_map_type = MapTileType.MapOnly;
        [XmlAttribute]
        public int original_zoom = 10;
        [XmlAttribute]
        public DateTime created = DateTime.Now;
        [XmlAttribute("group_name")]
        public string group = "";

        [XmlIgnore]
        public bool is_temporary = false; 

        int x, y;
        ImageDot imd = null;
        object tex = null;
        IGLFont fnt = null;

        [XmlIgnore]
        public BookMarkFactory owner = null;

        public Bookmark()
        {
        }

        public void initGLData()
        {
            imd = TextureFactory.singleton.getImg((TextureFactory.TexAlias)((int)TextureFactory.TexAlias.PinYellow + image_idx));
            tex = TextureFactory.singleton.getTex(imd);
            fnt = FontFactory.singleton.getGLFont(FontFactory.FontAlias.Sans10B);
        }

        public void makeId()
        {
            if (group.Length == 0)
                id = name;
            else
                id = "___/" + group + "/" + name;
        }

        #region ISprite Members

        public void draw(System.Drawing.Graphics gr)
        {
            
        }

        public void draw(System.Drawing.Graphics gr, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void glDraw(int centerx, int centery)
        {
            if (!shown)
                return;
            Point xy = owner.map.start_real_xy;
            xy.X = x - xy.X;
            xy.Y = y - xy.Y;

            xy.X -= centerx;
            xy.Y = centery - xy.Y;

            GML.device.pushMatrix();
            GML.device.color(Color.White);
            GML.device.translate(xy.X, xy.Y, 0);
            GML.device.rotateZ(-owner.map.angle);
            GML.device.texDrawBegin();
            GML.device.texFilter(tex, TexFilter.Pixel);
            GML.device.texDraw(tex, -imd.delta_x,
                imd.delta_y, dLevel, imd.img.Width, imd.img.Height);
            GML.device.texDrawEnd();
            GML.device.color(Color.Black);
//            xy = GML.translateToScene(xy);
            fnt.drawscene(name, xy.X, xy.Y, 0);
            GML.device.color(Color.White);
            GML.device.popMatrix();
        }

        [XmlIgnore]
        public int dLevel
        {
            get
            {
                return 1;
            }
            set
            {
                
            }
        }

        public void show()
        {
            shown = true;
            if (tex == null)
                initGLData();
            calculateXY(owner.map);
        }

        public void hide()
        {
            shown = false;
        }

        #endregion

        public void calculateXY(MapObject mapo)
        {
            Point xy;
            mapo.getXYByLonLat(lon, lat, out xy);
            x = xy.X;
            y = xy.Y;
        }
    }
}
