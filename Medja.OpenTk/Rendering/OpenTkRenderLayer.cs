using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Medja.Controls;
using Medja.Layers;
using Medja.Layers.Layouting;
using Medja.OpenTk.Eval;
using OpenTK.Graphics.OpenGL;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    // TODO dispose
    public class OpenTkRenderLayer : ILayer
    {
        private SkiaGLLayer _skia;
        private SKSurface _surface;
        private SKPaint _paint;
        private SKCanvas _canvas;

        public OpenTkRenderLayer()
        {
            _skia = new SkiaGLLayer();
        }

        public void Resize(int width, int height)
        {
            _skia.Resize(width, height);
        }

        public IEnumerable<ControlState> Apply(IEnumerable<ControlState> states)
        {
            using (_surface = _skia.CreateSurface())
            {
                using (_paint = new SKPaint
                {
                    // TODO dispose Typeface
/*                    Typeface = SKTypeface.FromFamilyName("Arial"),
                    TextSize = 12,*/
                    IsAntialias = true,
                    Color = SKColors.Orange,
                    Style = SKPaintStyle.StrokeAndFill,
                    //StrokeWidth = 10
                })
                {
                    _canvas = _surface.Canvas;
                    _canvas.Clear(SKColors.Gray);

                    Debug.WriteLine(" --- OpenTkRenderLayer --- ");

                    foreach (var state in states)
                    {
                        var item = state.Control;
                        var position = state.Position;

                        Debug.WriteLine(" - Control: " + item.GetType().Name);
                        Debug.WriteLine("      - Position: " + position);

                        if (item is Button b)
                        {
                            DrawRect(position);
                            DrawText(position, b.Text);
                        }
                        else
                            DrawRect(position);
                    }

                    _canvas.Flush();
                }
            }

            return states;
        }

        private void DrawText(PositionInfo positionInfo, string text)
        {
            _canvas.DrawText(text, positionInfo.X, positionInfo.Y, _paint);
        }

        private void DrawRect(PositionInfo positionInfo)
        {
            _canvas.DrawRect(GetSKRectFrom(positionInfo), _paint);
        }

        private SKRect GetSKRectFrom(PositionInfo positionInfo)
        {
            return new SKRect(positionInfo.X, positionInfo.Y, positionInfo.Width + positionInfo.X, positionInfo.Height + positionInfo.Y);
        }
    }
}
