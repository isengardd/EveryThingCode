using UnityEngine;
using FairyGUI.Utils;

namespace FairyGUI
{
    public static class FGUIExtend
    {
        private const float COLOR_MAX = 255.0F;

        public static void SetColor(this GImage image, int color)
        {
            image.color = ToolSet.ColorFromRGB(color);
        }

        public static void SetColor(GTextField text, int color)
        {
            text.color = ToolSet.ColorFromRGB(color);
        }

        public static void SetColor(GGraph graph, int color)
        {
            graph.color = ToolSet.ColorFromRGB(color);
        }

        public static void SetColor(GLoader loader, int color)
        {
            loader.color = ToolSet.ColorFromRGB(color);
        }

        // 目标的中心点坐标
        public static Vector2 CenterPosInStage(GObject gObject)
        {
            return FGUIExtend.LocalToStage(gObject, new Vector2(gObject.actualWidth / 2f, gObject.actualHeight / 2f));
        }

        // FGUI的local坐标转世界空间坐标
        public static Vector3 LocalToWorldSpace(GObject gObject)
        {
            Vector3 screenPos = LocalToStage(gObject, Vector2.zero);
            screenPos.y = Screen.height - screenPos.y;
            return StageCamera.main.ScreenToWorldPoint(screenPos);
        }

        public static Vector3 LocalToWorldSpace(Vector2 localPosition)
        {
            return StageCamera.main.ScreenToWorldPoint(new Vector3(localPosition.x, Screen.height - localPosition.y, 0));
        }

        // FGUI的世界空间坐标转local坐标
        public static Vector2 WorldSpaceToLocal(Vector3 position, GObject gObject, Camera camera)
        {
            Vector3 screenPos = camera.WorldToScreenPoint(position);
            screenPos.y = Screen.height - screenPos.y;
            return StageToLocal(screenPos, gObject);
        }

        // FGUI的local逻辑屏幕坐标转stage物理屏幕坐标
        public static Vector2 LocalToStage(GObject gObject, Vector2 offsetPos)
        {
            return gObject.LocalToGlobal(Vector2.zero + offsetPos);
        }

        // FGUI的stage物理屏幕坐标转local逻辑屏幕坐标
        public static Vector2 StageToLocal(Vector2 position, GObject gObject)
        {
            return gObject.GlobalToLocal(position);
        }
    }
}