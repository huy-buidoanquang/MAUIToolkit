namespace MAUIToolkit.Core;

internal class Matrix : IClone, IDisposable
{
    private float[] m_elements;

    private object m_skMatrix;

    //
    // Summary:
    //     Gets the elements.
    //
    // Value:
    //     The elements.
    public float[] Elements
    {
        get
        {
            return m_elements;
        }
        internal set
        {
            m_elements = value;
        }
    }

    //
    // Summary:
    //     Gets the off set X.
    //
    // Value:
    //     The off set X.
    public float OffsetX => m_elements[4];

    //
    // Summary:
    //     Gets the off set Y.
    //
    // Value:
    //     The off set Y.
    public float OffsetY => m_elements[5];

    internal object SkMatrix
    {
        get
        {
            return m_skMatrix;
        }
        set
        {
            m_skMatrix = value;
        }
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Drawing.Matrix class.
    public Matrix()
    {
        m_elements = new float[6] { 1f, 0f, 0f, 1f, 0f, 0f };
    }

    internal Matrix(Drawing.RectangleF rect, PointF[] points)
    {
        if (points.Length != 3)
        {
            throw new ArgumentException("Parameter is not valid.");
        }

        double[][] inverse = GetInverse(new double[3][]
        {
        new double[3] { rect.Left, rect.Top, 1.0 },
        new double[3] { rect.Right, rect.Top, 1.0 },
        new double[3] { rect.Left, rect.Bottom, 1.0 }
        });
        if (inverse == null)
        {
            m_elements = new float[6] { 1f, 0f, 0f, 1f, 0f, 0f };
        }
        else
        {
            m_elements = new float[6];
            m_elements[0] = (float)(inverse[0][0] * (double)points[0].X + inverse[0][1] * (double)points[1].X + inverse[0][2] * (double)points[2].X);
            m_elements[2] = (float)(inverse[1][0] * (double)points[0].X + inverse[1][1] * (double)points[1].X + inverse[1][2] * (double)points[2].X);
            m_elements[4] = (float)(inverse[2][0] * (double)points[0].X + inverse[2][1] * (double)points[1].X + inverse[2][2] * (double)points[2].X);
            m_elements[1] = (float)(inverse[0][0] * (double)points[0].Y + inverse[0][1] * (double)points[1].Y + inverse[0][2] * (double)points[2].Y);
            m_elements[3] = (float)(inverse[1][0] * (double)points[0].Y + inverse[1][1] * (double)points[1].Y + inverse[1][2] * (double)points[2].Y);
            m_elements[5] = (float)(inverse[2][0] * (double)points[0].Y + inverse[2][1] * (double)points[1].Y + inverse[2][2] * (double)points[2].Y);
        }
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Drawing.Matrix class.
    //
    // Parameters:
    //   m11:
    //     The M11.
    //
    //   m12:
    //     The M12.
    //
    //   m21:
    //     The M21.
    //
    //   m22:
    //     The M22.
    //
    //   dx:
    //     The dx.
    //
    //   dy:
    //     The dy.
    public Matrix(float m11, float m12, float m21, float m22, float dx, float dy)
        : this()
    {
        m_elements[0] = m11;
        m_elements[1] = m12;
        m_elements[2] = m21;
        m_elements[3] = m22;
        m_elements[4] = dx;
        m_elements[5] = dy;
    }

    //
    // Summary:
    //     Initializes a new instance of the MAUIToolkit.Core.Drawing.Matrix class.
    //
    // Parameters:
    //   elements:
    //     The elements.
    public Matrix(float[] elements)
    {
        m_elements = elements;
    }

    //
    // Summary:
    //     Translates the specified offset X.
    //
    // Parameters:
    //   offsetX:
    //     The offset X.
    //
    //   offsetY:
    //     The offset Y.
    public void Translate(float offsetX, float offsetY)
    {
        m_elements[4] = offsetX;
        m_elements[5] = offsetY;
    }

    internal PointF Transform(PointF point)
    {
        float x = point.X;
        float y = point.Y;
        float x2 = x * Elements[0] + y * Elements[2] + OffsetX;
        float y2 = x * Elements[1] + y * Elements[3] + OffsetY;
        return new PointF(x2, y2);
    }

    internal void TransformPoints(PointF[] points)
    {
        if (points != null && points.Length != 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = Transform(points[i]);
            }
        }
    }

    public void Multiply(Matrix matrix)
    {
        float[] array = new float[matrix.Elements.Length];
        array[0] = Elements[0] * matrix.Elements[0] + Elements[2] * matrix.Elements[1];
        array[1] = Elements[1] * matrix.Elements[0] + Elements[3] * matrix.Elements[1];
        array[2] = Elements[0] * matrix.Elements[2] + Elements[2] * matrix.Elements[3];
        array[3] = Elements[1] * matrix.Elements[2] + Elements[3] * matrix.Elements[3];
        array[4] = Elements[0] * matrix.Elements[4] + Elements[2] * matrix.Elements[5] + Elements[4];
        array[5] = Elements[1] * matrix.Elements[4] + Elements[3] * matrix.Elements[5] + Elements[5];
        for (int i = 0; i < array.Length; i++)
        {
            Elements[i] = array[i];
        }
    }

    internal static double GetDeterminant(double[][] inputMatrix)
    {
        double num = inputMatrix[0][0] * (inputMatrix[1][1] * inputMatrix[2][2] - inputMatrix[2][1] * inputMatrix[1][2]);
        double num2 = inputMatrix[1][0] * (inputMatrix[0][1] * inputMatrix[2][2] - inputMatrix[2][1] * inputMatrix[0][2]);
        double num3 = inputMatrix[2][0] * (inputMatrix[0][1] * inputMatrix[1][2] - inputMatrix[1][1] * inputMatrix[0][2]);
        return num - num2 + num3;
    }

    internal static double[][] GetInverse(double[][] inputMatrix)
    {
        double[][] array = new double[3][]
        {
        new double[3],
        new double[3],
        new double[3]
        };
        array[0][0] = inputMatrix[1][1] * inputMatrix[2][2] - inputMatrix[2][1] * inputMatrix[1][2];
        array[0][1] = -1.0 * (inputMatrix[1][0] * inputMatrix[2][2] - inputMatrix[2][0] * inputMatrix[1][2]);
        array[0][2] = inputMatrix[1][0] * inputMatrix[2][1] - inputMatrix[2][0] * inputMatrix[1][1];
        array[1][0] = -1.0 * (inputMatrix[0][1] * inputMatrix[2][2] - inputMatrix[2][1] * inputMatrix[0][2]);
        array[1][1] = inputMatrix[0][0] * inputMatrix[2][2] - inputMatrix[2][0] * inputMatrix[0][2];
        array[1][2] = -1.0 * (inputMatrix[0][0] * inputMatrix[2][1] - inputMatrix[2][0] * inputMatrix[0][1]);
        array[2][0] = inputMatrix[0][1] * inputMatrix[1][2] - inputMatrix[1][1] * inputMatrix[0][2];
        array[2][1] = -1.0 * (inputMatrix[0][0] * inputMatrix[1][2] - inputMatrix[1][0] * inputMatrix[0][2]);
        array[2][2] = inputMatrix[0][0] * inputMatrix[1][1] - inputMatrix[1][0] * inputMatrix[0][1];
        double determinant = GetDeterminant(inputMatrix);
        if (determinant == 0.0)
        {
            return null;
        }

        return GetTranspose(array, determinant);
    }

    internal static double[][] GetTranspose(double[][] inputMatrix)
    {
        return GetTranspose(inputMatrix, 1.0);
    }

    internal static double[][] GetTranspose(double[][] inputMatrix, double divedent)
    {
        double[][] array = new double[3][]
        {
        new double[3],
        new double[3],
        new double[3]
        };
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                array[j][i] = inputMatrix[i][j] / divedent;
            }
        }

        return array;
    }

    public void Dispose()
    {
        m_elements = null;
    }

    //
    // Summary:
    //     Clones this instance.
    public object Clone()
    {
        return new Matrix(m_elements);
    }
}
