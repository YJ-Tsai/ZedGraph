//============================================================================
//ZedGraph Class Library - A Flexible Line Graph/Bar Graph Library in C#
//Copyright (C) 2004  John Champion
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Drawing;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ZedGraph
{
	
	/// <summary>
	/// This class contains the data and methods for an individual curve within
	/// a graph pane.  It carries the settings for the curve including the
	/// key and item names, colors, symbols and sizes, linetypes, etc.
	/// </summary>
	/// 
	/// <author> John Champion
	/// modified by Jerry Vos </author>
	/// <version> $Revision: 3.31 $ $Date: 2006-03-05 07:28:16 $ </version>
	[Serializable]
	abstract public class CurveItem : ISerializable, ICloneable
	{
	
	#region Fields
		/// <summary>
		/// protected field that stores a legend label string for this
		/// <see cref="CurveItem"/>.  Use the public
		/// property <see cref="Label"/> to access this value.
		/// </summary>
		protected string	label;

		/// <summary>
		/// protected field that stores the special <see cref="FontSpec" /> to be used for
		/// the <see cref="Legend" /> entry of this <see cref="CurveItem" />.  Use the public
		/// property <see cref="FontSpec" /> to access this value;
		/// </summary>
		protected FontSpec	fontSpec;

		/// <summary>
		/// protected field that stores the boolean value that determines whether this
		/// <see cref="CurveItem"/> is on the left Y axis or the right Y axis (Y2).
		/// Use the public property <see cref="IsY2Axis"/> to access this value.
		/// </summary>
		protected bool		isY2Axis;

		/// <summary>
		/// protected field that stores the index number of the Y Axis to which this
		/// <see cref="CurveItem" /> belongs.  Use the public property <see cref="YAxisIndex" />
		/// to access this value.
		/// </summary>
		protected int		yAxisIndex;

		/// <summary>
		/// protected field that stores the boolean value that determines whether this
		/// <see cref="CurveItem"/> is visible on the graph.
		/// Use the public property <see cref="IsVisible"/> to access this value.
		/// Note that this value turns the curve display on or off, but it does not
		/// affect the display of the legend entry.  To hide the legend entry, you
		/// have to set <see cref="IsLegendLabelVisible"/> to false.
		/// </summary>
		protected bool		isVisible;
		/// <summary>
		/// protected field that stores a boolean value which allows you to override the normal
		/// ordinal axis behavior.  Use the public property <see cref="IsOverrideOrdinal"/> to
		/// access this value.
		/// </summary>
		protected bool		isOverrideOrdinal;
		/// <summary>
		/// protected field that stores the boolean value that determines whether the label
		/// for this <see cref="CurveItem"/> is visible in the legend.
		/// Use the public property <see cref="IsLegendLabelVisible"/> to access this value.
		/// Note that this value turns the legend entry display on or off, but it does not
		/// affect the display of the curve on the graph.  To hide the curve, you
		/// have to set <see cref="IsVisible"/> to false.
		/// </summary>
		protected bool		isLegendLabelVisible;
		
		/// <summary>
		/// The <see cref="IPointList"/> of value sets that
		/// represent this <see cref="CurveItem"/>.
		/// The size of this list determines the number of points that are
		/// plotted.  Note that values defined as
		/// System.Double.MaxValue are considered "missing" values
		/// (see <see cref="PointPair.Missing"/>),
		/// and are not plotted.  The curve will have a break at these points
		/// to indicate the values are missing.
		/// </summary>
		protected IPointList points;

		/// <summary>
		/// A tag object for use by the user.  This can be used to store additional
		/// information associated with the <see cref="CurveItem"/>.  ZedGraph does
		/// not use this value for any purpose.
		/// </summary>
		/// <remarks>
		/// Note that, if you are going to Serialize ZedGraph data, then any type
		/// that you store in <see cref="Tag"/> must be a serializable type (or
		/// it will cause an exception).
		/// </remarks>
		public object Tag;

	#endregion
	
	#region Constructors
		/// <summary>
		/// <see cref="CurveItem"/> constructor the pre-specifies the curve label, the
		/// x and y data values as a <see cref="IPointList"/>, the curve
		/// type (Bar or Line/Symbol), the <see cref="Color"/>, and the
		/// <see cref="SymbolType"/>. Other properties of the curve are
		/// defaulted to the values in the <see cref="GraphPane.Default"/> class.
		/// </summary>
		/// <param name="label">A string label (legend entry) for this curve</param>
		/// <param name="x">An array of double precision values that define
		/// the independent (X axis) values for this curve</param>
		/// <param name="y">An array of double precision values that define
		/// the dependent (Y axis) values for this curve</param>
		public CurveItem( string label, double[] x, double[] y ) :
				this( label, new PointPairList( x, y ) )
		{
		}
/*	
		public CurveItem( string label, int  y ) : this(  label, new IPointList( ) )
		{
		}
*/
		/// <summary>
		/// <see cref="CurveItem"/> constructor the pre-specifies the curve label, the
		/// x and y data values as a <see cref="IPointList"/>, the curve
		/// type (Bar or Line/Symbol), the <see cref="Color"/>, and the
		/// <see cref="SymbolType"/>. Other properties of the curve are
		/// defaulted to the values in the <see cref="GraphPane.Default"/> class.
		/// </summary>
		/// <param name="label">A string label (legend entry) for this curve</param>
		/// <param name="points">A <see cref="IPointList"/> of double precision value pairs that define
		/// the X and Y values for this curve</param>
		public CurveItem( string label, IPointList points )
		{
			Init( label );

			if ( points == null )
				this.points = new PointPairList();
			else
				//this.points = (IPointList) points.Clone();
				this.points = points;
		}
		
		/// <summary>
		/// Internal initialization routine thats sets some initial values to defaults.
		/// </summary>
		/// <param name="label">A string label (legend entry) for this curve</param>
		private void Init( string label )
		{
			this.label = label == null ? "" : label;
			this.fontSpec = null;
			this.isY2Axis = false;
			this.isVisible = true;
			this.isLegendLabelVisible = true;
			this.isOverrideOrdinal = false;
			this.Tag = null;
			this.yAxisIndex = 0;
		}
			
		/// <summary>
		/// <see cref="CurveItem"/> constructor that specifies the label of the CurveItem.
		/// This is the same as <c>CurveItem(label, null, null)</c>.
		/// <seealso cref="CurveItem( string, double[], double[] )"/>
		/// </summary>
		/// <param name="label">A string label (legend entry) for this curve</param>
		public CurveItem( string label ): this( label, null )
		{
		}
		 /// <summary>
		 /// 
		 /// </summary>
		public CurveItem(  )
		{
			Init( null );
		}
		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The CurveItem object from which to copy</param>
		public CurveItem( CurveItem rhs )
		{
			label = rhs.Label;
			isY2Axis = rhs.IsY2Axis;
			isVisible = rhs.IsVisible;
			isLegendLabelVisible = rhs.IsLegendLabelVisible;
			isOverrideOrdinal = rhs.isOverrideOrdinal;
			yAxisIndex = rhs.yAxisIndex;

			if ( rhs.fontSpec != null )
				this.fontSpec = rhs.fontSpec.Clone();
			else
				this.fontSpec = null;

			if ( rhs.Tag is ICloneable )
				this.Tag = ((ICloneable) rhs.Tag).Clone();
			else
				this.Tag = rhs.Tag;
			
			this.points = (IPointList) rhs.Points.Clone();
		}

		/// <summary>
		/// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
		/// calling the typed version of Clone.
		/// </summary>
		/// <remarks>
		/// Note that this method must be called with an explicit cast to ICloneable, and
		/// that it is inherently virtual.  For example:
		/// <code>
		/// ParentClass foo = new ChildClass();
		/// ChildClass bar = (ChildClass) ((ICloneable)foo).Clone();
		/// </code>
		/// Assume that ChildClass is inherited from ParentClass.  Even though foo is declared with
		/// ParentClass, it is actually an instance of ChildClass.  Calling the ICloneable implementation
		/// of Clone() on foo actually calls ChildClass.Clone() as if it were a virtual function.
		/// </remarks>
		/// <returns>A deep copy of this object</returns>
		object ICloneable.Clone()
		{
			throw new NotImplementedException( "Can't clone an abstract base type -- child types must implement ICloneable" );
			//return new PaneBase( this );
		}

	#endregion

	#region Serialization
		/// <summary>
		/// Current schema value that defines the version of the serialized file
		/// </summary>
		// Increased schema to 2 when IsOverrideOrdinal was added.
		// Increased schema to 3 when FontSpec was added.
		// Increased schema to 4 when YAxisIndex was added.
		public const int schema = 4;

		/// <summary>
		/// Constructor for deserializing objects
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
		/// </param>
		/// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
		/// </param>
		protected CurveItem( SerializationInfo info, StreamingContext context )
		{
			// The schema value is just a file version parameter.  You can use it to make future versions
			// backwards compatible as new member variables are added to classes
			int sch = info.GetInt32( "schema" );

			label = info.GetString( "label" );
			isY2Axis = info.GetBoolean( "isY2Axis" );
			isVisible = info.GetBoolean( "isVisible" );
			isLegendLabelVisible = info.GetBoolean( "isLegendLabelVisible" );

			if ( sch >= 2 )
				isOverrideOrdinal = info.GetBoolean( "isOverrideOrdinal" );

			// Data Points are always stored as a PointPairList, regardless of the
			// actual original type (which could be anything that supports IPointList).
			points = (PointPairList) info.GetValue( "points", typeof(PointPairList) );

			Tag = info.GetValue( "Tag", typeof(object) );

			if ( sch >= 3 )
				fontSpec = (FontSpec) info.GetValue( "fontSpec", typeof(FontSpec) );

			if ( sch >= 4 )
				yAxisIndex = info.GetInt32( "yAxisIndex" );

		}
		/// <summary>
		/// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
		/// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public virtual void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			info.AddValue( "schema", schema );
			info.AddValue( "label", label );
			info.AddValue( "isY2Axis", isY2Axis );
			info.AddValue( "isVisible", isVisible );
			info.AddValue( "isLegendLabelVisible", isLegendLabelVisible );
			info.AddValue( "isOverrideOrdinal", isOverrideOrdinal );

			// if points is already a PointPairList, use it
			// otherwise, create a new PointPairList so it can be serialized
			PointPairList list;
			if ( points is PointPairList )
				list = points as PointPairList;
			else
				list = new PointPairList( points );

			info.AddValue( "points", list );
			info.AddValue( "Tag", Tag );
			info.AddValue( "fontSpec", fontSpec );
			info.AddValue( "yAxisIndex", yAxisIndex );
		}
	#endregion
	
	#region Properties
		/// <summary>
		/// A text string that represents the <see cref="ZedGraph.Legend"/>
		/// entry for the this
		/// <see cref="CurveItem"/> object
		/// </summary>
		/// <seealso cref="CurveItem.FontSpec" />
		public string Label
		{
			get { return label; }
			set { label = value;}
		}

		/// <summary>
		/// Gets or sets the special <see cref="FontSpec" /> to be used for
		/// the <see cref="Legend" /> entry of this <see cref="CurveItem" />.
		/// </summary>
		/// <remarks>
		/// This property defaults to null, indicating that the legend entry will use the
		/// default <see cref="FontSpec" /> as defined for the <see cref="Legend" /> object.
		/// If this property is non-null, then the special font will be used to draw the
		/// legend label for this <see cref="CurveItem" />.
		/// </remarks>
		public FontSpec	FontSpec
		{
			get { return fontSpec; }
			set { fontSpec = value; }
		}

		/// <summary>
		/// The <see cref="Line"/>/<see cref="Symbol"/>/<see cref="Bar"/> 
		/// color (FillColor for the Bar).  This is a common access to
		/// <see cref="ZedGraph.Line.Color"/>, <see cref="ZedGraph.Border.Color"/>, and
		/// <see cref="ZedGraph.Fill.Color"/> properties for this curve.
		/// </summary>
		public Color Color
		{
			get
			{
				if ( this is BarItem )
					return ((BarItem) this).Bar.Fill.Color;
				else if ( this is LineItem && ((LineItem) this).Line.IsVisible )
					return ((LineItem) this).Line.Color;
				else if ( this is LineItem )
					return ((LineItem) this).Symbol.Border.Color;
				else if ( this is ErrorBarItem )
					return ((ErrorBarItem) this).ErrorBar.Color;
				else if ( this is HiLowBarItem )
					return ((HiLowBarItem) this).Bar.Fill.Color;
				else
					return Color.Empty;
			}
			set 
			{
				if ( this is BarItem )
				{
					((BarItem) this).Bar.Fill.Color = value;
				}
				else if ( this is LineItem )
				{
					((LineItem) this).Line.Color			= value;
					((LineItem) this).Symbol.Border.Color	= value;
					((LineItem) this).Symbol.Fill.Color		= value;
				}
				else if ( this is ErrorBarItem )
					((ErrorBarItem) this).ErrorBar.Color = value;
				else if ( this is HiLowBarItem )
					((HiLowBarItem) this).Bar.Fill.Color = value;
			}
		}

		/// <summary>
		/// Determines whether this <see cref="CurveItem"/> is visible on the graph.
		/// Note that this value turns the curve display on or off, but it does not
		/// affect the display of the legend entry.  To hide the legend entry, you
		/// have to set <see cref="IsLegendLabelVisible"/> to false.
		/// </summary>
		public bool IsVisible
		{
			get { return isVisible; }
			set { isVisible = value; }
		}
		/// <summary>
		/// Determines whether the label for this <see cref="CurveItem"/> is visible in the legend.
		/// Note that this value turns the legend entry display on or off, but it does not
		/// affect the display of the curve on the graph.  To hide the curve, you
		/// have to set <see cref="IsVisible"/> to false.
		/// </summary>
		public bool IsLegendLabelVisible
		{
			get { return isLegendLabelVisible; }
			set { isLegendLabelVisible = value; }
		}

		/// <summary>
		/// Gets or sets a value which allows you to override the normal
		/// ordinal axis behavior.
		/// </summary>
		/// <remarks>
		/// Normally for an ordinal axis type, the actual data values corresponding to the ordinal
		/// axis will be ignored (essentially they are replaced by ordinal values, e.g., 1, 2, 3, etc).
		/// If IsOverrideOrdinal is true, then the user data values will be used (even if they don't
		/// make sense).  Fractional values are allowed, such that a value of 1.5 is between the first and
		/// second ordinal position, etc.
		/// </remarks>
		/// <seealso cref="AxisType.Ordinal"/>
		/// <seealso cref="AxisType.Text"/>
		public bool IsOverrideOrdinal
		{
			get { return isOverrideOrdinal; }
			set { isOverrideOrdinal = value; }
		}

		/// <summary>
		/// Gets or sets a value that determines which Y axis this <see cref="CurveItem"/>
		/// is assigned to.
		/// </summary>
		/// <remarks>
		/// The
		/// <see cref="ZedGraph.YAxis"/> is on the left side of the graph and the
		/// <see cref="ZedGraph.Y2Axis"/> is on the right side.  Assignment to an axis
		/// determines the scale that is used to draw the curve on the graph.  Note that
		/// this value is used in combination with the <see cref="YAxisIndex" /> to determine
		/// which of the Y Axes (if there are multiples) this curve belongs to.
		/// </remarks>
		/// <value>true to assign the curve to the <see cref="ZedGraph.Y2Axis"/>,
		/// false to assign the curve to the <see cref="ZedGraph.YAxis"/></value>
		public bool IsY2Axis
		{
			get { return isY2Axis; }
			set { isY2Axis = value; }
		}
		
		/// <summary>
		/// Gets or sets the index number of the Y Axis to which this
		/// <see cref="CurveItem" /> belongs.
		/// </summary>
		/// <remarks>
		/// This value is essentially an index number into the <see cref="GraphPane.YAxisList" />
		/// or <see cref="GraphPane.Y2AxisList" />, depending on the setting of
		/// <see cref="IsY2Axis" />.
		/// </remarks>
		public int YAxisIndex
		{
			get { return yAxisIndex; }
			set { yAxisIndex = value; }
		}

		/// <summary>
		/// Determines whether this <see cref="CurveItem"/>
		/// is a <see cref="BarItem"/>.  This does not include <see cref="HiLowBarItem"/>'s
		/// or <see cref="ErrorBarItem"/>'s.
		/// </summary>
		/// <value>true for a bar chart, or false for a line or pie graph</value>
		public bool IsBar
		{
			get { return this is BarItem; }
		}
		
		/// <summary>
		/// Determines whether this <see cref="CurveItem"/>
		/// is a <see cref="PieItem"/>.
		/// </summary>
		/// <value>true for a pie chart, or false for a line or bar graph</value>
		public bool IsPie
		{
			get { return this is PieItem; }
		}
		
		/// <summary>
		/// Determines whether this <see cref="CurveItem"/>
		/// is a <see cref="LineItem"/>.
		/// </summary>
		/// <value>true for a line chart, or false for a bar type</value>
		public bool IsLine
		{
			get { return this is LineItem; }
		}

		/// <summary>
		/// Gets a flag indicating if the Z data range should be included in the axis scaling calculations.
		/// </summary>
		/// <param name="pane">The parent <see cref="GraphPane" /> of this <see cref="CurveItem" />.
		/// </param>
		/// <value>true if the Z data are included, false otherwise</value>
		abstract internal bool IsZIncluded( GraphPane pane );
		
		/// <summary>
		/// Gets a flag indicating if the X axis is the independent axis for this <see cref="CurveItem" />
		/// </summary>
		/// <param name="pane">The parent <see cref="GraphPane" /> of this <see cref="CurveItem" />.
		/// </param>
		/// <value>true if the X axis is independent, false otherwise</value>
		abstract internal bool IsXIndependent( GraphPane pane );
		
		/// <summary>
		/// Readonly property that gives the number of points that define this
		/// <see cref="CurveItem"/> object, which is the number of points in the
		/// <see cref="Points"/> data collection.
		/// </summary>
		public int NPts
		{
			get 
			{
				if ( this.points == null )
					return 0;
				else
					return this.points.Count;
			}
		}
		
		/// <summary>
		/// The <see cref="IPointList"/> of X,Y point sets that represent this
		/// <see cref="CurveItem"/>.
		/// </summary>
		public IPointList Points
		{
			get { return points; }
			set { points = value; }
		}

		/// <summary>
		/// An accessor for the <see cref="PointPair"/> datum for this <see cref="CurveItem"/>.
		/// Index is the ordinal reference (zero based) of the point.
		/// </summary>
		public PointPair this[int index]
		{
			get
			{
				if ( this.points == null )
					return new PointPair( PointPair.Missing, PointPair.Missing );
				else
					return ( this.points )[index];
			}
		}
	#endregion
	
	#region Rendering Methods
		/// <summary>
		/// Do all rendering associated with this <see cref="CurveItem"/> to the specified
		/// <see cref="Graphics"/> device.  This method is normally only
		/// called by the Draw method of the parent <see cref="ZedGraph.CurveList"/>
		/// collection object.
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see cref="ZedGraph.GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="pos">The ordinal position of the current <see cref="Bar"/>
		/// curve.</param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="ZedGraph.GraphPane"/> object using the
		/// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		abstract public void Draw( Graphics g, GraphPane pane, int pos, float scaleFactor  );
		
		/// <summary>
		/// Draw a legend key entry for this <see cref="CurveItem"/> at the specified location.
		/// This abstract base method passes through to <see cref="BarItem.DrawLegendKey"/> or
		/// <see cref="LineItem.DrawLegendKey"/> to do the rendering.
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
        /// <param name="pane">
        /// A reference to the <see cref="ZedGraph.GraphPane"/> object that is the parent or
        /// owner of this object.
        /// </param>
        /// <param name="rect">The <see cref="RectangleF"/> struct that specifies the
        /// location for the legend key</param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="ZedGraph.GraphPane"/> object using the
		/// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		abstract public void DrawLegendKey( Graphics g, GraphPane pane, RectangleF rect, float scaleFactor );
		
	#endregion

	#region Utility Methods

		/// <summary>
		/// Add a single x,y coordinate point to the end of the points collection for this curve.
		/// </summary>
		/// <param name="x">The X coordinate value</param>
		/// <param name="y">The Y coordinate value</param>
		public void AddPoint( double x, double y )
		{
			this.AddPoint( new PointPair( x, y ) );
		}

		/// <summary>
		/// Add a <see cref="PointPair"/> object to the end of the points collection for this curve.
		/// </summary>
		/// <remarks>
		/// This method will only work if the <see cref="IPointList" /> instance reference
		/// at <see cref="Points" /> supports the <see cref="IPointListEdit" /> interface.
		/// Otherwise, it does nothing.
		/// </remarks>
		/// <param name="point">A reference to the <see cref="PointPair"/> object to
		/// be added</param>
		public void AddPoint( PointPair point )
		{
			if ( this.points == null )
				this.Points = new PointPairList();

			if ( this.points is IPointListEdit )
				( points as IPointListEdit ).Add( point );
			else
				throw new NotImplementedException();
		}

		/// <summary>
		/// Clears the points from this <see cref="CurveItem"/>.  This is the same
		/// as <c>CurveItem.Points.Clear()</c>.
		/// </summary>
		/// <remarks>
		/// This method will only work if the <see cref="IPointList" /> instance reference
		/// at <see cref="Points" /> supports the <see cref="IPointListEdit" /> interface.
		/// Otherwise, it does nothing.
		/// </remarks>
		public void Clear()
		{
			if ( this.points is IPointListEdit )
				(points as IPointListEdit).Clear();
			else
				throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a single point from this <see cref="CurveItem" />.
		/// </summary>
		/// <remarks>
		/// This method will only work if the <see cref="IPointList" /> instance reference
		/// at <see cref="Points" /> supports the <see cref="IPointListEdit" /> interface.
		/// Otherwise, it does nothing.
		/// </remarks>
		/// <param name="index">The ordinal position of the point to be removed.</param>
		public void RemovePoint( int index )
		{
			if ( this.points is IPointListEdit )
				(points as IPointListEdit).Remove( index );
			else
				throw new NotImplementedException();
		}


		/// <summary>
		/// Get the Y Axis instance (either <see cref="YAxis" /> or <see cref="Y2Axis" />) to
		/// which this <see cref="CurveItem" /> belongs.
		/// </summary>
		/// <remarks>
		/// This method safely retrieves a Y Axis instance from either the <see cref="GraphPane.YAxisList" />
		/// or the <see cref="GraphPane.Y2AxisList" /> using the values of <see cref="YAxisIndex" /> and
		/// <see cref="IsY2Axis" />.  If the value of <see cref="YAxisIndex" /> is out of bounds, the
		/// default <see cref="YAxis" /> or <see cref="Y2Axis" /> is used.
		/// </remarks>
		/// <param name="pane">The <see cref="GraphPane" /> object to which this curve belongs.</param>
		/// <returns>Either a <see cref="YAxis" /> or <see cref="Y2Axis" /> to which this
		/// <see cref="CurveItem" /> belongs.
		/// </returns>
		public Axis GetYAxis( GraphPane pane )
		{
			if ( this.isY2Axis )
				return pane.Y2AxisList[ yAxisIndex ];
			else
				return pane.YAxisList[ yAxisIndex ];
		}

		/// <summary>
		/// Get the index of the Y Axis in the <see cref="YAxis" /> or <see cref="Y2Axis" /> list to
		/// which this <see cref="CurveItem" /> belongs.
		/// </summary>
		/// <remarks>
		/// This method safely retrieves a Y Axis index into either the <see cref="GraphPane.YAxisList" />
		/// or the <see cref="GraphPane.Y2AxisList" /> using the values of <see cref="YAxisIndex" /> and
		/// <see cref="IsY2Axis" />.  If the value of <see cref="YAxisIndex" /> is out of bounds, the
		/// default <see cref="YAxis" /> or <see cref="Y2Axis" /> is used, which is index zero.
		/// </remarks>
		/// <param name="pane">The <see cref="GraphPane" /> object to which this curve belongs.</param>
		/// <returns>An integer value indicating which index position in the list applies to this
		/// <see cref="CurveItem" />
		/// </returns>
		public int GetYAxisIndex( GraphPane pane )
		{
			if ( yAxisIndex >= 0 &&
					yAxisIndex < ( this.isY2Axis ? pane.Y2AxisList.Count : pane.YAxisList.Count ) )
				return yAxisIndex;
			else
				return 0;
		}

		/// <summary>
		/// Loads some pseudo unique colors/symbols into this CurveItem.  This
		/// is the same as <c>MakeUnique(ColorSymbolRotator.StaticInstance)</c>.
		/// <seealso cref="ColorSymbolRotator.StaticInstance"/>
		/// <seealso cref="ColorSymbolRotator"/>
		/// <seealso cref="MakeUnique(ColorSymbolRotator)"/>
		/// </summary>
		public void MakeUnique()
		{
			this.MakeUnique( ColorSymbolRotator.StaticInstance );
		}

		/// <summary>
		/// Loads some pseudo unique colors/symbols into this CurveItem.  This
		/// is mainly useful for differentiating a set of new CurveItems without
		/// having to pick your own colors/symbols.
		/// <seealso cref="MakeUnique(ColorSymbolRotator)"/>
		/// </summary>
		/// <param name="rotator">
		/// The <see cref="ColorSymbolRotator"/> that is used to pick the color
		///  and symbol for this method call.
		/// </param>
		virtual public void MakeUnique( ColorSymbolRotator rotator )
		{
			this.Color = rotator.NextColor;
		}
	
		/// <summary>
		/// Go through the list of <see cref="PointPair"/> data values for this <see cref="CurveItem"/>
		/// and determine the minimum and maximum values in the data.
		/// </summary>
		/// <param name="xMin">The minimum X value in the range of data</param>
		/// <param name="xMax">The maximum X value in the range of data</param>
		/// <param name="yMin">The minimum Y value in the range of data</param>
		/// <param name="yMax">The maximum Y value in the range of data</param>
		/// <param name="ignoreInitial">ignoreInitial is a boolean value that
		/// affects the data range that is considered for the automatic scale
		/// ranging (see <see cref="GraphPane.IsIgnoreInitial"/>).  If true, then initial
		/// data points where the Y value is zero are not included when
		/// automatically determining the scale <see cref="Axis.Min"/>,
		/// <see cref="Axis.Max"/>, and <see cref="Axis.Step"/> size.  All data after
		/// the first non-zero Y value are included.
		/// </param>
		/// <param name="isBoundedRanges">
		/// Determines if the auto-scaled axis ranges will subset the
		/// data points based on any manually set scale range values.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see cref="GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <seealso cref="GraphPane.IsBoundedRanges"/>
		virtual public void GetRange( 	out double xMin, out double xMax,
										out double yMin, out double yMax,
										bool ignoreInitial,
										bool isBoundedRanges,
										GraphPane pane )
		{
			// The lower and upper bounds of allowable data for the X values.  These
			// values allow you to subset the data values.  If the X range is bounded, then
			// the resulting range for Y will reflect the Y values for the points within the X
			// bounds.
			double xLBound = double.MinValue;
			double xUBound = double.MaxValue;
			double yLBound = double.MinValue;
			double yUBound = double.MaxValue;

			Axis yAxis = this.GetYAxis( pane );

			if ( isBoundedRanges )
			{
				xLBound = pane.XAxis.scale.lBound;
				xUBound = pane.XAxis.scale.uBound;
				yLBound = yAxis.scale.lBound;
				yUBound = yAxis.scale.uBound;
			}


			bool isZIncluded = this.IsZIncluded( pane );
			bool isXIndependent = this.IsXIndependent( pane );
			bool isXLog = pane.XAxis.IsLog;
			bool isYLog = yAxis.IsLog;

			// initialize the values to outrageous ones to start
			xMin = yMin = Double.MaxValue;
			xMax = yMax = Double.MinValue;
		
			// Loop over each point in the arrays
			//foreach ( PointPair point in this.Points )
			for ( int i=0; i<this.Points.Count; i++ )
			{
				PointPair point = this.Points[i];

				double curX = point.X;
				double curY = point.Y;
				double curZ = point.Z;

				bool outOfBounds = curX < xLBound || curX > xUBound ||
					curY < yLBound || curY > yUBound ||
					( isZIncluded && isXIndependent && ( curZ < yLBound || curZ > yUBound ) ) ||
					( isZIncluded && !isXIndependent && ( curZ < xLBound || curZ > xUBound ) ) ||
					( curX <= 0 && isXLog ) || ( curY <= 0 && isYLog );
			
				// ignoreInitial becomes false at the first non-zero
				// Y value
				if (	ignoreInitial && curY != 0 &&
						curY != PointPair.Missing )
					ignoreInitial = false;
			
				if ( 	!ignoreInitial &&
						!outOfBounds &&
						curX != PointPair.Missing &&
						curY != PointPair.Missing )
				{
					if ( curX < xMin )
						xMin = curX;
					if ( curX > xMax )
						xMax = curX;
					if ( curY < yMin )
						yMin = curY;
					if ( curY > yMax )
						yMax = curY;

					if ( isZIncluded && isXIndependent && curZ != PointPair.Missing )
					{
						if ( curZ < yMin )
							yMin = curZ;
						if ( curZ > yMax )
							yMax = curZ;
					}
					else if ( isZIncluded && curZ != PointPair.Missing )
					{
						if ( curZ < xMin )
							xMin = curZ;
						if ( curZ > xMax )
							xMax = curZ;
					}
				}
			}	
		}

		/// <summary>Returns a reference to the <see cref="Axis"/> object that is the "base"
		/// (independent axis) from which the values are drawn. </summary>
		/// <remarks>
		/// This property is determined by the value of <see cref="GraphPane.BarBase"/> for
		/// <see cref="BarItem"/>, <see cref="ErrorBarItem"/>, and <see cref="HiLowBarItem"/>
		/// types.  It is always the X axis for regular <see cref="LineItem"/> types.
		/// Note that the <see cref="GraphPane.BarBase" /> setting can override the
		/// <see cref="IsY2Axis" /> and <see cref="YAxisIndex" /> settings for bar types
		/// (this is because all the bars that are clustered together must share the
		/// same base axis).
		/// </remarks>
		/// <seealso cref="BarBase"/>
		/// <seealso cref="ValueAxis"/>
		public virtual Axis BaseAxis( GraphPane pane )
		{
			BarBase barBase;

			if ( this is BarItem || this is ErrorBarItem || this is HiLowBarItem )
				barBase = pane.BarBase;
			else
				barBase = BarBase.X;

			if ( barBase == BarBase.X )
				return pane.XAxis;
			else if ( barBase == BarBase.Y )
				return pane.YAxis;
			else
				return pane.Y2Axis;

		}
		/// <summary>Returns a reference to the <see cref="Axis"/> object that is the "value"
		/// (dependent axis) from which the points are drawn. </summary>
		/// <remarks>
		/// This property is determined by the value of <see cref="GraphPane.BarBase"/> for
		/// <see cref="BarItem"/>, <see cref="ErrorBarItem"/>, and <see cref="HiLowBarItem"/>
		/// types.  It is always the Y axis for regular <see cref="LineItem"/> types.
		/// </remarks>
		/// <seealso cref="BarBase"/>
		/// <seealso cref="BaseAxis"/>
		public virtual Axis ValueAxis( GraphPane pane )
		{
			BarBase barBase;

			if ( this is BarItem || this is ErrorBarItem || this is HiLowBarItem )
				barBase = pane.BarBase;
			else
				barBase = BarBase.X;

			if ( barBase == BarBase.X )
			{
				return GetYAxis( pane );
				//if ( isY2Axis )
				//	return pane.Y2Axis;
				//else
				//	return pane.YAxis;
			}
			else
				return pane.XAxis;
		}

		/// <summary>
		/// Calculate the width of each bar, depending on the actual bar type
		/// </summary>
		/// <returns>The width for an individual bar, in pixel units</returns>
		public float GetBarWidth( GraphPane pane )
		{
			// Total axis width = 
			// npts * ( nbars * ( bar + bargap ) - bargap + clustgap )
			// cg * bar = cluster gap
			// npts = max number of points in any curve
			// nbars = total number of curves that are of type IsBar
			// bar = bar width
			// bg * bar = bar gap
			// therefore:
			// totwidth = npts * ( nbars * (bar + bg*bar) - bg*bar + cg*bar )
			// totwidth = bar * ( npts * ( nbars * ( 1 + bg ) - bg + cg ) )
			// solve for bar

			float barWidth;

			if ( this is ErrorBarItem )
				barWidth = (float) ( ((ErrorBarItem)this).ErrorBar.Symbol.Size *
						pane.CalcScaleFactor() );
			else if ( this is HiLowBarItem )
				barWidth = (float) ( ((HiLowBarItem)this).Bar.GetBarWidth( pane,
						((HiLowBarItem)this).BaseAxis(pane), pane.CalcScaleFactor() ) );
			else // BarItem or LineItem
			{
				// For stacked bar types, the bar width will be based on a single bar
				float numBars = 1.0F;
				if ( pane.BarType == BarType.Cluster || pane.BarType == BarType.ClusterHiLow )
					numBars = pane.CurveList.NumBars;
					
				float denom = numBars * ( 1.0F + pane.MinBarGap ) - pane.MinBarGap + pane.MinClusterGap;
				if ( denom <= 0 )
					denom = 1;
				barWidth = pane.GetClusterWidth() / denom;
			}

			if ( barWidth <= 0 )
				return 1;

			return barWidth;
		}
		

	#endregion
	
	#region Inner classes	
		/// <summary>
		/// Compares <see cref="CurveItem"/>'s based on the point value at the specified
		/// index and for the specified axis.
		/// <seealso cref="System.Collections.ArrayList.Sort()"/>
		/// </summary>
		public class Comparer : IComparer 
		{
			private int index;
			private SortType sortType;
			
			/// <summary>
			/// Constructor for Comparer.
			/// </summary>
			/// <param name="type">The axis type on which to sort.</param>
			/// <param name="index">The index number of the point on which to sort</param>
			public Comparer( SortType type, int index )
			{
				this.sortType = type;
				this.index = index;
			}
			
			/// <summary>
			/// Compares two <see cref="CurveItem"/>s using the previously specified index value
			/// and axis.  Sorts in descending order.
			/// </summary>
			/// <param name="l">Curve to the left.</param>
			/// <param name="r">Curve to the right.</param>
			/// <returns>-1, 0, or 1 depending on l.X's relation to r.X</returns>
			int IComparer.Compare( object l, object r ) 
			{
				if (l == null && r == null )
					return 0;
				else if (l == null && r != null ) 
					return -1;
				else if (l != null && r == null) 
					return 1;

				CurveItem lc = (CurveItem) l;
				CurveItem rc = (CurveItem) r;
				if ( rc != null && rc.NPts <= index )
					r = null;
				if ( lc != null && lc.NPts <= index )
					l = null;
						
				double lVal, rVal;

				if ( sortType == SortType.XValues )
				{
					lVal = System.Math.Abs( lc[index].X );
					rVal = System.Math.Abs( rc[index].X );
				}
				else
				{
					lVal = System.Math.Abs( lc[index].Y );
					rVal = System.Math.Abs( rc[index].Y );
				}
				
				if ( lVal == PointPair.Missing || Double.IsInfinity( lVal ) || Double.IsNaN( lVal ) )
					l = null;
				if ( rVal == PointPair.Missing || Double.IsInfinity( rVal ) || Double.IsNaN( rVal ) )
					r = null;
					
				if ( (l == null && r == null) || ( System.Math.Abs( lVal - rVal ) < 1e-10 ) )
					return 0;
				else if (l == null && r != null ) 
					return -1;
				else if (l != null && r == null) 
					return 1;
				else
					return rVal < lVal ? -1 : 1;
			}
		}
	
	#endregion

	}
}



