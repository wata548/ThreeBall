namespace ThreeBall;

public struct Vector2(float pX = 0,float pY = 0) {
	public float X { get; set; } = pX;
	public float Y { get; set; } = pY;

	public override string ToString() => $"({X}, {Y})";

	public static readonly Vector2 Zero = new(0, 0);
	public static readonly Vector2 One = new(1, 1);
	public static readonly Vector2 Left = new(-1, 0);
	public static readonly Vector2 Right = new(1, 0);
	public static readonly Vector2 Up = new(0, 1);
	public static readonly Vector2 Down = new(0, -1);

	public static Vector2 Lerp(Vector2 pLhs, Vector2 pRhs, float pT) =>
		(1 - pT) * pLhs + pT * pRhs;
	
#region Operators

	public readonly float SqDistance => X * X + Y * Y;
	public readonly float Distance => MathF.Sqrt(SqDistance);
	public readonly Vector2 Normalized {
		get => new Vector2(X, Y) / Distance;
	}
	public readonly float Dot(Vector2 pOther) => X * pOther.X + Y * pOther.Y;
	public readonly float Cross(Vector2 pOther) => X * pOther.Y - Y * pOther.X;
	
	public void Rotate(float pDelta) {
		var rad = pDelta * MathF.PI / 180f;
		var (cos, sin) = (MathF.Cos(rad), MathF.Sin(rad));
		(X, Y) = (cos * X - sin * Y, cos * Y + sin * X);
	}
	
	public void Mapped(Func<float, float> pFunc) => (X, Y) = (pFunc(X), pFunc(Y));
	public readonly Vector2 Map(Func<float, float> pFunc) => new(pFunc(X), pFunc(Y));
	public void SwapXY() => (X, Y) = (Y, X);
	public Vector2 SwapedXY() => new(Y, X);
	public static Vector2 operator -(Vector2 lhs) => new(-lhs.X, -lhs.Y);
	public static Vector2 operator +(Vector2 lhs, Vector2 rhs) =>
		new(lhs.X + rhs.X, lhs.Y + rhs.Y);
	public static Vector2 operator -(Vector2 lhs, Vector2 rhs) =>
		new(lhs.X - rhs.X, lhs.Y - rhs.Y);

	public static Vector2 operator *(Vector2 lhs, Vector2 rhs) =>
		new(lhs.X * rhs.X, lhs.Y * rhs.Y);

	public static Vector2 operator /(Vector2 lhs, Vector2 rhs) =>
		new(lhs.X / rhs.X, lhs.Y / rhs.Y);

	public static Vector2 operator +(float lhs, Vector2 rhs) =>
		new(lhs + rhs.X, lhs + rhs.Y);

	public static Vector2 operator +(Vector2 lhs, float rhs) =>
		new(lhs.X + rhs, lhs.Y + rhs);

	public static Vector2 operator -(float lhs, Vector2 rhs) =>
		new(lhs - rhs.X, lhs - rhs.Y);

	public static Vector2 operator -(Vector2 lhs, float rhs) =>
		new(lhs.X - rhs, lhs.Y - rhs);

	public static Vector2 operator *(float lhs, Vector2 rhs) =>
		new(lhs * rhs.X, lhs * rhs.Y);

	public static Vector2 operator *(Vector2 lhs, float rhs) =>
		new(lhs.X * rhs, lhs.Y * rhs);

	public static Vector2 operator /(float lhs, Vector2 rhs) =>
		new(lhs / rhs.X, lhs / rhs.Y);

	public static Vector2 operator /(Vector2 lhs, float rhs) =>
		new(lhs.X / rhs, lhs.Y / rhs);

	public static bool operator ==(Vector2 lhs, Vector2 rhs) =>
		lhs.Equals(rhs);
	public static bool operator !=(Vector2 lhs, Vector2 rhs) => 
		!(lhs == rhs);
	
	public bool Equals(Vector2 pRhs) => 
		(X, Y) == (pRhs.X, pRhs.Y);
	
	public override bool Equals( object? obj) =>
		obj is Vector2 rhs && Equals(rhs);

	public override int GetHashCode() =>
		HashCode.Combine(X, Y);

#endregion
}