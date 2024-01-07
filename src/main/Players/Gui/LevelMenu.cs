﻿using Godot;
using System;

using Jumpvalley.Tweening;

namespace Jumpvalley.Players.Gui
{
    /// <summary>
    /// Code for the level menu, a type of menu that's typically displayed only when the player is playing a level.
    /// </summary>
    public partial class LevelMenu: IDisposable
    {
        /// <summary>
        /// The root node of the level menu
        /// </summary>
        public Control ActualNode { get; private set; }

        /// <summary>
        /// Tween handling the transparency of the menu's items, including its background panel
        /// </summary>
        private SceneTreeTween transparencyTween;

        /// <summary>
        /// Tween handling the appearance of the menu's background panel
        /// </summary>
        private SceneTreeTween backgroundSizeTween;

        /// <summary>
        /// The Godot control displaying the menu's background
        /// </summary>
        public Control BackgroundNode { get; private set; }

        /// <summary>
        /// The text label displaying the menu's title
        /// </summary>
        public Label TitleLabel { get; private set; }

        /// <summary>
        /// The text label displaying the menu's subtitle
        /// </summary>
        public Label SubtitleLabel { get; private set; }

        private bool _isShowing;

        /// <summary>
        /// Whether or not the level menu should be visible.
        /// Toggling this property will run the animation for showing/hiding the menu.
        /// </summary>
        public bool IsShowing
        {
            get => _isShowing;
            set
            {
                _isShowing = value;

                if (value)
                {
                    transparencyTween.Speed = 1;

                    if (backgroundSizeTween != null)
                    {
                        backgroundSizeTween.Speed = 1;
                    }
                }
                else
                {
                    transparencyTween.Speed = -1;

                    if (backgroundSizeTween != null)
                    {
                        backgroundSizeTween.Speed = -1;
                    }
                }

                transparencyTween.Resume();
                if (backgroundSizeTween != null)
                {
                    backgroundSizeTween.Resume();
                }
            }
        }

        /// <summary>
        /// Constructs a new instance of the level menu handler.
        /// </summary>
        /// <param name="actualNode">The root node of the level menu</param>
        /// <param name="tree">The scene tree that the level menu is in</param>
        /// <exception cref="ArgumentNullException"></exception>
        public LevelMenu(Control actualNode, SceneTree tree)
        {
            if (actualNode == null) throw new ArgumentNullException("actualNode", "The actualNode argument (argument #1) cannot be null.");

            ActualNode = actualNode;
            BackgroundNode = actualNode.GetNode<Control>("Background");
            TitleLabel = actualNode.GetNode<Label>("Title");
            SubtitleLabel = actualNode.GetNode<Label>("Subtitle");

            transparencyTween = new SceneTreeTween(0.25, Tween.TransitionType.Linear, Tween.EaseType.Out, tree);
            transparencyTween.InitialValue = 0;
            transparencyTween.FinalValue = 1;
            transparencyTween.OnStep += (object o, float frac) =>
            {
                ActualNode.Visible = frac > 0;
                Color modulate = ActualNode.Modulate;
                modulate.A = frac;
                ActualNode.Modulate = modulate;
            };

            if (BackgroundNode != null)
            {
                backgroundSizeTween = new SceneTreeTween(0.5, Tween.TransitionType.Quint, Tween.EaseType.Out, tree);
                backgroundSizeTween.InitialValue = -20;
                backgroundSizeTween.FinalValue = 0;
                backgroundSizeTween.OnStep += (object o, float frac) =>
                {
                    float sizeOffset = (float)(backgroundSizeTween.GetCurrentValue() * 0.5);
                    BackgroundNode.OffsetLeft = sizeOffset;
                    BackgroundNode.OffsetRight = sizeOffset;
                    BackgroundNode.OffsetTop = sizeOffset;
                    BackgroundNode.OffsetBottom = sizeOffset;
                };
            }

            actualNode.Visible = false;

            IsShowing = false;
        }

        public void Dispose()
        {
            transparencyTween.Dispose();
            backgroundSizeTween.Dispose();
        }
    }
}
