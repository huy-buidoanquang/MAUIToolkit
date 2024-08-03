using MAUIToolkit.Graphics.Core;
using System.Windows.Input;

namespace MAUIToolkit.Sample.Views
{
    public class SliderView : Graphics.Core.GraphicsView, ISliderController, ISlider
    {
		public static readonly BindableProperty MinimumProperty = BindableProperty.Create(nameof(Minimum), typeof(double), typeof(SliderView), 0d, coerceValue: (bindable, value) =>
        {
            var slider = (SliderView)bindable;
            slider.Value = slider.Value.Clamp((double)value, slider.Maximum);
            return value;
        });

        public static readonly BindableProperty MaximumProperty = BindableProperty.Create(nameof(Maximum), typeof(double), typeof(SliderView), 1d, coerceValue: (bindable, value) =>
        {
            var slider = (SliderView)bindable;
            slider.Value = slider.Value.Clamp(slider.Minimum, (double)value);
            return value;
        });

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(SliderView), 0d, BindingMode.TwoWay, coerceValue: (bindable, value) =>
        {
            var slider = (SliderView)bindable;
            return ((double)value).Clamp(slider.Minimum, slider.Maximum);
        }, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var slider = (SliderView)bindable;
            slider.ValueChanged?.Invoke(slider, new ValueChangedEventArgs((double)oldValue, (double)newValue));
        });

        /// <summary>Bindable property for <see cref="MinimumTrackColor"/>.</summary>
        public static readonly BindableProperty MinimumTrackColorProperty = BindableProperty.Create(nameof(MinimumTrackColor), typeof(Color), typeof(SliderView), null);

        /// <summary>Bindable property for <see cref="MaximumTrackColor"/>.</summary>
        public static readonly BindableProperty MaximumTrackColorProperty = BindableProperty.Create(nameof(MaximumTrackColor), typeof(Color), typeof(SliderView), null);

        /// <summary>Bindable property for <see cref="ThumbColor"/>.</summary>
        public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(SliderView), null);

        /// <summary>Bindable property for <see cref="ThumbImageSource"/>.</summary>
        public static readonly BindableProperty ThumbImageSourceProperty = BindableProperty.Create(nameof(ThumbImageSource), typeof(ImageSource), typeof(SliderView), default(ImageSource));

        /// <summary>Bindable property for <see cref="DragStartedCommand"/>.</summary>
        public static readonly BindableProperty DragStartedCommandProperty = BindableProperty.Create(nameof(DragStartedCommand), typeof(ICommand), typeof(SliderView), default(ICommand));

        /// <summary>Bindable property for <see cref="DragCompletedCommand"/>.</summary>
        public static readonly BindableProperty DragCompletedCommandProperty = BindableProperty.Create(nameof(DragCompletedCommand), typeof(ICommand), typeof(SliderView), default(ICommand));

        readonly Lazy<PlatformConfigurationRegistry<SliderView>> _platformConfigurationRegistry;

        public SliderView(double min, double max, double val) : this()
        {
            if (min >= max)
                throw new ArgumentOutOfRangeException(nameof(min));

            if (max > Minimum)
            {
                Maximum = max;
                Minimum = min;
            }
            else
            {
                Minimum = min;
                Maximum = max;
            }
            Value = val.Clamp(min, max);
        }

        public Color MinimumTrackColor
        {
            get { return (Color)GetValue(MinimumTrackColorProperty); }
            set { SetValue(MinimumTrackColorProperty, value); }
        }

        public Color MaximumTrackColor
        {
            get { return (Color)GetValue(MaximumTrackColorProperty); }
            set { SetValue(MaximumTrackColorProperty, value); }
        }

        public Color ThumbColor
        {
            get { return (Color)GetValue(ThumbColorProperty); }
            set { SetValue(ThumbColorProperty, value); }
        }

        public ImageSource ThumbImageSource
        {
            get { return (ImageSource)GetValue(ThumbImageSourceProperty); }
            set { SetValue(ThumbImageSourceProperty, value); }
        }

        public ICommand DragStartedCommand
        {
            get { return (ICommand)GetValue(DragStartedCommandProperty); }
            set { SetValue(DragStartedCommandProperty, value); }
        }

        public ICommand DragCompletedCommand
        {
            get { return (ICommand)GetValue(DragCompletedCommandProperty); }
            set { SetValue(DragCompletedCommandProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        public event EventHandler DragStarted;
        public event EventHandler DragCompleted;

        void ISliderController.SendDragStarted()
        {
            if (IsEnabled)
            {
                DragStartedCommand?.Execute(null);
                DragStarted?.Invoke(this, null);
            }
        }

        void ISliderController.SendDragCompleted()
        {
            if (IsEnabled)
            {
                DragCompletedCommand?.Execute(null);
                DragCompleted?.Invoke(this, null);
            }
        }

        /// <inheritdoc/>
        public IPlatformElementConfiguration<T, SliderView> On<T>() where T : IConfigPlatform
        {
            return _platformConfigurationRegistry.Value.On<T>();
        }

        IImageSource ISlider.ThumbImageSource => ThumbImageSource;

        void ISlider.DragCompleted()
        {
            (this as ISliderController).SendDragCompleted();
        }

        void ISlider.DragStarted()
        {
            (this as ISliderController).SendDragStarted();
        }
        private readonly MaterialSliderDrawable _drawable;

        public SliderView()
        {
            _drawable = new MaterialSliderDrawable();
            _platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<SliderView>>(() => new PlatformConfigurationRegistry<SliderView>(this));
        }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);

            _drawable.DrawBackground(canvas, dirtyRect, this);
            _drawable.DrawTrackProgress(canvas, dirtyRect, this);
            _drawable.DrawThumb(canvas, dirtyRect, this);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var desiredSize = _drawable.GetDesiredSize(this, widthConstraint, heightConstraint);
            return new SizeRequest(desiredSize);
        }
    }
}
