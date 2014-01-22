using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PathNormalizeTool
{
    /// <summary>
    /// メイン ViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// 入力済みフラグ
        /// </summary>
        public bool IsInputted { get { return !string.IsNullOrEmpty(this.pathData); } }

        #region PathData プロパティ
        /// <summary>
        /// Path Data
        /// </summary>
        private string pathData;

        /// <summary>
        /// Path Data
        /// </summary>
        public string PathData
        {
            get { return this.pathData; }
            set
            {
                this.Set<string>("PathData", ref this.pathData, value);
                this.Geometry = Geometry.Parse(this.pathData);
                this.Width = (this.Geometry.Bounds.Right - this.Geometry.Bounds.Left);
                this.Height = (this.Geometry.Bounds.Bottom - this.Geometry.Bounds.Top);
                this.UpdatePathData();
                this.RaisePropertyChanged("PathData");
                this.RaisePropertyChanged("IsInputted");
            }
        }
        #endregion //PathData プロパティ

        #region Width プロパティ
        /// <summary>
        /// Width
        /// </summary>
        private double width;

        /// <summary>
        /// Width
        /// </summary>
        public double Width
        {
            get { return this.width; }
            set 
            { 
                this.Set<double>("Width", ref this.width, value);
                this.UpdatePathData();
            }
        }
        #endregion //Width プロパティ

        #region Height プロパティ
        /// <summary>
        /// Height
        /// </summary>
        private double height;

        /// <summary>
        /// Height
        /// </summary>
        public double Height
        {
            get { return this.height; }
            set 
            { 
                this.Set<double>("Height", ref this.height, value);
                this.UpdatePathData();
            }
        }
        #endregion //Height プロパティ

        #region Left プロパティ
        /// <summary>
        /// Left
        /// </summary>
        private double left;

        /// <summary>
        /// Left
        /// </summary>
        public double Left
        {
            get { return this.left; }
            set
            {
                this.Set<double>("Left", ref this.left, value);
                this.UpdatePathData();
                this.RaisePropertyChanged("Margin");
            }
        }
        #endregion //Left プロパティ

        #region Top プロパティ
        /// <summary>
        /// Top
        /// </summary>
        private double top;

        /// <summary>
        /// Top
        /// </summary>
        public double Top
        {
            get { return this.top; }
            set
            {
                this.Set<double>("Top", ref this.top, value);
                this.UpdatePathData();
                this.RaisePropertyChanged("Margin");
            }
        }
        #endregion //Top プロパティ

        #region Right プロパティ
        /// <summary>
        /// Right
        /// </summary>
        private double right;

        /// <summary>
        /// Right
        /// </summary>
        public double Right
        {
            get { return this.right; }
            set
            {
                this.Set<double>("Right", ref this.right, value);
                this.RaisePropertyChanged("Margin");
            }
        }
        #endregion //Right プロパティ

        #region Bottom プロパティ
        /// <summary>
        /// Bottom
        /// </summary>
        private double bottom;

        /// <summary>
        /// Bottom
        /// </summary>
        public double Bottom
        {
            get { return this.bottom; }

            set
            {
                this.Set<double>("Bottom", ref this.bottom, value);
                this.RaisePropertyChanged("Margin");
            }
        }
        #endregion //Bottom プロパティ

        /// <summary>
        /// Margin
        /// </summary>
        public Thickness Margin
        {
            get { return new Thickness(0, 0, this.left, this.top); }
        }

        /// <summary>
        /// Geometry
        /// </summary>
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Path の整形
        /// </summary>
        public void UpdatePathData()
        {
            try
            {
                if (this.Geometry == null)
                {
                    return;
                }

                var pathGeometory = (this.Geometry.Clone() as StreamGeometry).GetFlattenedPathGeometry();
                var scaled = new PathGeometry();
                double scaleX = (this.Geometry.Bounds.Right - this.Geometry.Bounds.Left) / this.Width;
                double scaleY = (this.Geometry.Bounds.Bottom - this.Geometry.Bounds.Top) / this.Height;

                foreach (var figure in pathGeometory.Figures)
                {
                    var newFigure = new PathFigure();

                    newFigure.StartPoint = new Point(
                        ((figure.StartPoint.X - this.Geometry.Bounds.Left) / scaleX) + this.Left,
                        ((figure.StartPoint.Y - this.Geometry.Bounds.Top) / scaleY) + this.Top
                        );
                    foreach (PolyLineSegment segment in figure.Segments)
                    {
                        var newSegment = new PolyLineSegment();
                        foreach (var point in segment.Points)
                        {
                            newSegment.Points.Add(new Point(
                                ((point.X - this.Geometry.Bounds.Left) / scaleX) + this.Left,
                                ((point.Y - this.Geometry.Bounds.Top) / scaleY) + this.Top
                                ));
                        }
                        newFigure.Segments.Add(newSegment);
                    }
                    scaled.Figures.Add(newFigure);
                }
                scaled.Freeze();
                this.pathData = scaled.ToString();
                this.RaisePropertyChanged("PathData");
                this.PathGeometry = scaled;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 整形後 Path
        /// </summary>
        private Geometry pathGeometry;

        /// <summary>
        /// 整形後 Path
        /// </summary>
        public Geometry PathGeometry
        {
            get { return this.pathGeometry; }
            set { this.Set<Geometry>("PathGeometry", ref this.pathGeometry, value); }
        }
    }
}
