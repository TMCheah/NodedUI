using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NodedUI
{
    public enum NodeType
    {
        Input,
        Output
    }

    /// <summary>
    /// Interaction logic for Node.xaml
    /// </summary>
    public partial class Node : UserControl
    {
        //public NodeType NodeType { get; set; }

        private bool isDragging = false;
        private bool isInside = false;
        private Point startPoint;
        private Point endPoint;
        private Point controlPoint1;
        private Point controlPoint2;

        // Using a DependencyProperty as the backing store for NodeTypes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodeTypesProperty =
            DependencyProperty.Register("NodeType", typeof(NodeType), typeof(Node), new PropertyMetadata(NodeType.Input));

        public NodeType NodeType
        {
            get { return (NodeType)GetValue(NodeTypesProperty); }
            set { SetValue(NodeTypesProperty, value); }
        }

        public Node()
        {
            InitializeComponent();
            MouseDown += Node_MouseDown;
            MouseMove += Node_MouseMove;
            MouseUp += Node_MouseUp;
            MouseEnter += Node_MouseEnter;
            MouseLeave += Node_MouseLeave;
        }

        private void Node_MouseEnter(object sender, MouseEventArgs e)
        {
            isInside = true;
        }

        private void Node_MouseLeave(object sender, MouseEventArgs e)
        {
            isInside = false;
        }

        private void Node_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (NodeType == NodeType.Input)
                return;


            isDragging = true;
            startPoint = e.GetPosition(this);
            Mouse.Capture(this);

        }

        private void Node_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging || NodeType == NodeType.Input)
                return;

            endPoint = e.GetPosition(this);

            double distance = endPoint.X - startPoint.X;
            double offsetX = distance * 0.3;
            controlPoint1 = new Point(startPoint.X + offsetX, startPoint.Y);
            controlPoint2 = new Point(endPoint.X - offsetX, endPoint.Y);
            InvalidateVisual();
        }

        private void Node_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!isDragging || NodeType == NodeType.Input)
                return;

            isDragging = false;
            Mouse.Capture(null);

            Point currentPoint = e.GetPosition((UIElement)sender);
            Point endPoint = new Point(Canvas.GetLeft(this) + currentPoint.X, Canvas.GetTop(this) + currentPoint.Y);

            // Perform hit testing with other input nodes
            UIElement element = FindElementUnderMouse(node, currentPoint);

            if (element is Node inputNode &&
                        inputNode.NodeType == NodeType.Input &&
                        IsPointInConnectorRange(inputNode, endPoint))
            {
                // Handle the connection between nodes here
                // For example, update the appearance of the connector line or establish a connection data structure
                //break;
                Console.WriteLine("connected");
            }


            //var parentContainer = VisualTreeHelper.GetParent(this) as Panel;
            //if (parentContainer != null)
            //{
            //    foreach (UIElement element in parentContainer.Children)
            //    {
            //        if (element is Node inputNode &&
            //            inputNode.NodeType == NodeType.Input &&
            //            IsPointInConnectorRange(inputNode, endPoint))
            //        {
            //            // Handle the connection between nodes here
            //            // For example, update the appearance of the connector line or establish a connection data structure
            //            break;
            //        }
            //    }
            //}
        }

        private bool IsPointInConnectorRange(Node node, Point point)
        {
            double centerX = Canvas.GetLeft(node) + (node.ActualWidth / 2);
            double centerY = Canvas.GetTop(node) + (node.ActualHeight / 2);
            double radiusX = node.ActualWidth / 2;
            double radiusY = node.ActualHeight / 2;

            double normalizedX = (point.X - centerX) / radiusX;
            double normalizedY = (point.Y - centerY) / radiusY;

            return (normalizedX * normalizedX + normalizedY * normalizedY) <= 1;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (NodeType == NodeType.Output && isDragging)
            {
                var bezierSegment = new BezierSegment(controlPoint1, controlPoint2, endPoint, true);
                var pathFigure = new PathFigure(startPoint, new[] { bezierSegment }, false);
                var pathGeometry = new PathGeometry(new[] { pathFigure });
                var pen = new Pen(Brushes.Black, 2);
                drawingContext.DrawGeometry(null, pen, pathGeometry);
            }
        }

        private UIElement FindElementUnderMouse(Visual parent, Point mousePosition)
        {
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(parent, mousePosition);
            return (UIElement)(hitTestResult?.VisualHit);
        }
    }
}
