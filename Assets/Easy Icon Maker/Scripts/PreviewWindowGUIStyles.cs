using UnityEngine;

namespace EasyIconMaker
{
    public class PreviewGUIStyles
    {
        private static GUIStyle createButtonStyle;
        public static GUIStyle CreateButtonStyle
        {
            get
            {
                if (createButtonStyle == null)
                    createButtonStyle = new GUIStyle(GUI.skin.FindStyle("button"));

                createButtonStyle.fontSize = 14;
                createButtonStyle.font = Resources.Load<Font>("Fonts/Montserrat-Regular");


                return createButtonStyle;
            }
        }

        private static GUIStyle greenFont;
        public static GUIStyle GreenFont
        {
            get
            {
                if (greenFont == null)
                    greenFont = new GUIStyle();

                greenFont.normal.textColor = new Color(0.05f, 0.5f, 0.0f);
                greenFont.fontStyle = FontStyle.Bold;

                return greenFont;
            }
        }

        private static GUIStyle yellowFont;
        public static GUIStyle YellowFont
        {
            get
            {
                if (yellowFont == null)
                    yellowFont = new GUIStyle();

                yellowFont.normal.textColor = new Color(0.45f, 0.45f, 0.0f);
                yellowFont.fontStyle = FontStyle.Bold;

                return yellowFont;
            }
        }

        private static GUIStyle sectionHeaderFont;
        public static GUIStyle SectionHeaderFont
        {
            get
            {
                if (sectionHeaderFont == null)
                    sectionHeaderFont = new GUIStyle();

                sectionHeaderFont.fontSize = 16;
                sectionHeaderFont.font = Resources.Load<Font>("Fonts/Montserrat-Regular");
                sectionHeaderFont.margin.top = 10;
                sectionHeaderFont.alignment = TextAnchor.MiddleCenter;

                return sectionHeaderFont;
            }
        }

        private static GUIStyle sectionIcon;
        public static GUIStyle SectionIcon
        {
            get
            {
                if (sectionIcon == null)
                    sectionIcon = new GUIStyle();

                sectionIcon.margin.top = 4;

                return sectionIcon;
            }
        }

        private static GUIStyle sectionCollapseArrow;
        public static GUIStyle SectionCollapseArrow
        {
            get
            {
                if (sectionCollapseArrow == null)
                    sectionCollapseArrow = new GUIStyle();

                sectionCollapseArrow.margin.top = 6;

                return sectionCollapseArrow;
            }
        }
    }
}