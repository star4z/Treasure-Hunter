using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Representation of 2D vectors and points.</para>
  /// </summary>
  public struct P
  {
    /// <summary>
    ///   <para>X component of the vector.</para>
    /// </summary>
    public int x;
    /// <summary>
    ///   <para>Y component of the vector.</para>
    /// </summary>
    public int y;

    public int this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.x;
          case 1:
            return this.y;
          default:
            throw new IndexOutOfRangeException("Invalid P index!");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.x = value;
            break;
          case 1:
            this.y = value;
            break;
          default:
            throw new IndexOutOfRangeException("Invalid P index!");
        }
      }
    }

    /// <summary>
    ///   <para>Returns this vector with a magnitude of 1 (Read Only).</para>
    /// </summary>
    public P normalized
    {
      get
      {
        P P = new P(this.x, this.y);
        P.Normalize();
        return P;
      }
    }

    /// <summary>
    ///   <para>Returns the length of this vector (Read Only).</para>
    /// </summary>
    public int magnitude
    {
      get
      {
        return (int) Mathf.Sqrt((float) ((double) this.x * (double) this.x + (double) this.y * (double) this.y));
      }
    }

    /// <summary>
    ///   <para>Returns the squared length of this vector (Read Only).</para>
    /// </summary>
    public int sqrMagnitude
    {
      get
      {
        return (int) ((double) this.x * (double) this.x + (double) this.y * (double) this.y);
      }
    }

    /// <summary>
    ///   <para>Shorthand for writing P(0, 0).</para>
    /// </summary>
    public static P zero
    {
      get
      {
        return new P(0, 0);
      }
    }

    /// <summary>
    ///   <para>Shorthand for writing P(1, 1).</para>
    /// </summary>
    public static P one
    {
      get
      {
        return new P(1, 1);
      }
    }

    /// <summary>
    ///   <para>Shorthand for writing P(0, 1).</para>
    /// </summary>
    public static P up
    {
      get
      {
        return new P(0, 1);
      }
    }

    /// <summary>
    ///   <para>Shorthand for writing P(0, -1).</para>
    /// </summary>
    public static P down
    {
      get
      {
        return new P(0, -1);
      }
    }

    /// <summary>
    ///   <para>Shorthand for writing P(-1, 0).</para>
    /// </summary>
    public static P left
    {
      get
      {
        return new P(-1, 0);
      }
    }

    /// <summary>
    ///   <para>Shorthand for writing P(1, 0).</para>
    /// </summary>
    public static P right
    {
      get
      {
        return new P(1, 0);
      }
    }

    /// <summary>
    ///   <para>Constructs a new vector with given x, y components.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public P(int x, int y)
    {
      this.x = x;
      this.y = y;
    }

    public static P operator +(P a, P b)
    {
      return new P(a.x + b.x, a.y + b.y);
    }

    public static P operator -(P a, P b)
    {
      return new P(a.x - b.x, a.y - b.y);
    }

    public static P operator -(P a)
    {
      return new P(-a.x, -a.y);
    }

    public static P operator *(P a, int d)
    {
      return new P(a.x * d, a.y * d);
    }

    public static P operator *(int d, P a)
    {
      return new P(a.x * d, a.y * d);
    }

    public static P operator /(P a, float d)
    {
      return new P((int) (a.x / d),(int) (a.y / d));
    }

    public static bool operator ==(P lhs, P rhs)
    {
      return (double) P.SqrMagnitude(lhs - rhs) < 9.99999943962493E-11;
    }

    public static bool operator !=(P lhs, P rhs)
    {
      return (double) P.SqrMagnitude(lhs - rhs) >= 9.99999943962493E-11;
    }

    /// <summary>
    ///   <para>Set x and y components of an existing P.</para>
    /// </summary>
    /// <param name="new_x"></param>
    /// <param name="new_y"></param>
    public void Set(int new_x, int new_y)
    {
      this.x = new_x;
      this.y = new_y;
    }

    /// <summary>
    ///   <para>Moves a point current towards target.</para>
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    /// <param name="maxDistanceDelta"></param>
    public static P MoveTowards(P current, P target, float maxDistanceDelta)
    {
      P P = target - current;
      int magnitude = P.magnitude;
      if ((double) magnitude <= (double) maxDistanceDelta || (double) magnitude == 0.0)
        return target;
      return current + P / magnitude * (int) maxDistanceDelta;
    }

    /// <summary>
    ///   <para>Multiplies two vectors component-wise.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static P Scale(P a, P b)
    {
      return new P(a.x * b.x, a.y * b.y);
    }

    /// <summary>
    ///   <para>Multiplies every component of this vector by the same component of scale.</para>
    /// </summary>
    /// <param name="scale"></param>
    public void Scale(P scale)
    {
      this.x *= scale.x;
      this.y *= scale.y;
    }

    /// <summary>
    ///   <para>Makes this vector have a magnitude of 1.</para>
    /// </summary>
    public void Normalize()
    {
      int magnitude = this.magnitude;
      if ((double) magnitude > 9.99999974737875E-06)
        this = this / magnitude;
      else
        this = P.zero;
    }

    /// <summary>
    ///   <para>Returns a nicely formatted string for this vector.</para>
    /// </summary>
    /// <param name="format"></param>
    public override string ToString()
    {
      return "(" + x + ", " + y + ")";
    }

    public override int GetHashCode()
    {
      return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
    }

    public override bool Equals(object other)
    {
      if (!(other is P))
        return false;
      P P = (P) other;
      if (this.x.Equals(P.x))
        return this.y.Equals(P.y);
      return false;
    }

    /// <summary>
    ///   <para>Reflects a vector off the vector defined by a normal.</para>
    /// </summary>
    /// <param name="inDirection"></param>
    /// <param name="inNormal"></param>
    public static P Reflect(P inDirection, P inNormal)
    {
      return -2 * P.Dot(inNormal, inDirection) * inNormal + inDirection;
    }

    /// <summary>
    ///   <para>Dot Product of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    public static int Dot(P lhs, P rhs)
    {
      return (int) ((double) lhs.x * (double) rhs.x + (double) lhs.y * (double) rhs.y);
    }

    /// <summary>
    ///   <para>Returns the angle in degrees between from and to.</para>
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public static float Angle(P from, P to)
    {
      return Mathf.Acos(Mathf.Clamp(P.Dot(from.normalized, to.normalized), -1f, 1f)) * 57.29578f;
    }

    /// <summary>
    ///   <para>Returns the distance between a and b.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static int Distance(P a, P b)
    {
      return (a - b).magnitude;
    }

    /// <summary>
    ///   <para>Returns a copy of vector with its magnitude clamped to maxLength.</para>
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="maxLength"></param>
    public static P ClampMagnitude(P vector, int maxLength)
    {
      if ((double) vector.sqrMagnitude > (double) maxLength * (double) maxLength)
        return vector.normalized * maxLength;
      return vector;
    }

    public static int SqrMagnitude(P a)
    {
      return (int) ((double) a.x * (double) a.x + (double) a.y * (double) a.y);
    }

    public int SqrMagnitude()
    {
      return (int) ((double) this.x * (double) this.x + (double) this.y * (double) this.y);
    }

    /// <summary>
    ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    public static P Min(P lhs, P rhs)
    {
      return new P(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y));
    }

    /// <summary>
    ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    public static P Max(P lhs, P rhs)
    {
      return new P(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y));
    }

  }
}