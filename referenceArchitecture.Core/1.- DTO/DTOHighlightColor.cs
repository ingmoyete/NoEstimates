using NoEstimates.Core.Enums;
using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOHighlightColor : DTOBase
    {
        public int Id { get; set; }
        public int TaskId { get; set; }

        public int Color { get; set; }
        public HighlightColor EnumColor { get { return (HighlightColor)Color;  } }

        public string BlueBallClass { get { return isActiveColor(HighlightColor.Blue); } }
        public string YellowBallClass { get { return isActiveColor(HighlightColor.Yellow); } }
        public string RedBallClass { get { return isActiveColor(HighlightColor.Red); } }

        public string PanelColorClass
        {
            get
            {
                string ret = string.Empty;
                switch (EnumColor)
                {
                    case HighlightColor.Blue: ret = "panel-primary";
                        break;
                    case HighlightColor.Yellow: ret = "panel-yellow";
                        break;
                    case HighlightColor.Red: ret = "panel-red";
                        break;
                    default:
                        ret = "panel-primary";
                        break;
                }
                return ret;

            }
        }

        private string isActiveColor(HighlightColor color)
        {
            var isActiveColor = EnumColor == color ? "colorWhite activeColor" : "";
            return isActiveColor;
        }
    }
}
