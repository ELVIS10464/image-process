using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct RGB24
{
    public byte B;
    public byte G;
    public byte R;
};

[StructLayout(LayoutKind.Sequential)]
public struct iIPoint
{
    public int x;
    public int y;
}

[StructLayout(LayoutKind.Sequential)]
public struct iDPoint
{
    public double x;
    public double y;
}

[StructLayout(LayoutKind.Sequential)]
public struct iFPoint
{
    public float x;
    public float y;
}

[StructLayout(LayoutKind.Sequential)]
public struct mRect
{
	public int  top;
	public int  down;
	public int  left;
	public int  right;
}

[StructLayout(LayoutKind.Sequential)]
public struct POINTGROUP
{
    public iFPoint Pos1;
    public iFPoint Pos2;
    public iFPoint Pos3;
    public iFPoint Pos4;
};

[StructLayout(LayoutKind.Sequential)]
public struct iMinBox
{
    public float X_center;
    public float Y_center;
    public float Height;
    public float width;
    public float Angle;
    public POINTGROUP POS;
};

[StructLayout(LayoutKind.Sequential)]
public struct ColMean	                                           
{
    public double R;
    public double G;
    public double B;
};

[StructLayout(LayoutKind.Sequential)]
public struct NCCFind
{
	public int		Width;
	public int		Height;

	public double     CX;
	public double     CY;
	public double	    Angle;
	public double	    Scale;
	public double	    Score;
}

[StructLayout(LayoutKind.Sequential)]
public struct sGeometryFind
{
    public double x;
	public double y;
	public double Score;
	public double Angle;
	public double Scale;
}


[StructLayout(LayoutKind.Sequential)]
public struct iCircle_Measured
{
    public iDPoint Cp;
    public double Diameter;
    public double Roundness;
	public double PL_Difference;
}

[StructLayout(LayoutKind.Sequential)]
public struct iLine_Measured
{
    public iIPoint p1;
    public iIPoint p2;
    public double A;
    public double B;
    public double C;
    public double angle;
}

[StructLayout(LayoutKind.Sequential)]
public struct iRectangle_Measured
{
    public iIPoint p1;
    public iIPoint p2;
    public iIPoint p3;
    public iIPoint p4;
    public iDPoint cp;
    public double angle;
    public double width;
    public double height;
};

[StructLayout(LayoutKind.Sequential)]
public struct iTriangle_Measured
{
    public iIPoint crosspoint;
    public double   angle;
};

[StructLayout(LayoutKind.Sequential)]
public  struct iObject_BlobFEA
{
	public long		area;
	public double  	XCenter;
	public double	YCenter;
	public iMinBox	MinRect;

    public int Blob_index;
}

[StructLayout(LayoutKind.Sequential)]
public  struct iMCLLC_Measured
{
    public iIPoint p1;
    public iIPoint p2;
    public double distance;
}

[StructLayout(LayoutKind.Sequential)]
public struct iBaseROI
{
    public int OrgX;
    public int OrgY;
    public int Width;
    public int Height;
}

[StructLayout(LayoutKind.Sequential)]
public struct iAdvanceROI
{
    public int m_center_x;
    public int m_center_y;
    public int m_width;
    public int m_height;
    public double m_rotation_angle;
}

[StructLayout(LayoutKind.Sequential)]
public struct iRectangleROI
{
    public int m_center_x;
    public int m_center_y;
    public int m_width;
    public int m_height;
    public int m_gap;
    public double m_rotation_angle;
}

[StructLayout(LayoutKind.Sequential)]
public struct iAdvPairROI
{
    public int m_center_x;
    public int m_center_y;
    public int m_advPair_width_1;
    public int m_advPair_width_2;
    public int m_width_1;
    public int m_height_1;
    public int m_width_2;
    public int m_height_2;
    public double m_rotation_angle;
}

[StructLayout(LayoutKind.Sequential)]
public struct iTriAdvPairROI
{
    public int m_center_x;
    public int m_center_y;
    public int m_begin_width;
    public int m_begin_height;
    public int m_end_width;
    public int m_end_height;
    public double m_begin_angle;
    public double m_end_angle;
    public double m_begin_radius;
    public double m_end_radius;
}

[StructLayout(LayoutKind.Sequential)]
public struct iRingROI
{
    public int m_center_x;
    public int m_center_y;
    public int m_gap;
    public double m_radius;
}

[StructLayout(LayoutKind.Sequential)]
public struct iAdvRingROI
{
    public int m_center_x;
    public int m_center_y;
    public int m_ring_gap;
    public double m_begin_angle;
    public double m_end_angle;
    public double m_radius;
}

[StructLayout(LayoutKind.Sequential)]
public struct iCircleROI
{
    public int m_center_x;
    public int m_center_y;
    public double m_radius;
}

public enum iROI_Type : uint
{
    iBase = 0,
    iAdvance,
    iAdvPair,
    iRectangle,
    iCircle,
    iRing,
    iAdvRing,
    iTriAdvPair,
    iNULL = 9
};

public enum IgnoreType : uint

{
	UnknownType		=   0,
	InnerRectangle	= 	1,
	OuterRectangle	= 	2,
	InnerCircle		=	3,
	OuterCircle		=	4
}; 

[StructLayout(LayoutKind.Sequential)]
public struct sIgnore_Para
{
	public iAdvanceROI	m_advance;
	public iCircleROI	m_circle;
	public IgnoreType	m_type;
}

public enum eTransitionType : int
{
    BlackToWhite = 0,
    WhiteToBlack = 1,
    Both = 2
};