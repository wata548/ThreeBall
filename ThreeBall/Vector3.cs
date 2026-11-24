namespace ThreeBall;

public struct Vector3(float pX = 0,float pY = 0,float pZ = 0) {
	public float X { get; set; } = pX;
	public float Y { get; set; } = pY;
	public float Z { get; set; } = pZ;

	public override string ToString() => $"({X}, {Y}, {Z})";

	public static readonly Vector3 Zero = new(0, 0, 0);
	public static readonly Vector3 One = new(1, 1, 1);
	public static readonly Vector3 Left = new(-1, 0, 0);
	public static readonly Vector3 Right = new(1, 0, 0);
	public static readonly Vector3 Up = new(0, 1, 0);
	public static readonly Vector3 Down = new(0, -1, 0);
	public static readonly Vector3 Backward = new(0, 0, 1);
	public static readonly Vector3 Forward = new(0, 0, -1);
	
#region Operators

	public readonly float SqDistance => X * X + Y * Y + Z * Z;
	public readonly float Distance => MathF.Sqrt(SqDistance);
	public readonly Vector3 Normalized {
		get => new Vector3(X, Y, Z) / Distance;
	}
	public readonly float Dot(Vector3 pOther) => X * pOther.X + Y * pOther.Y + Z * pOther.Z;
	public readonly Vector3 Cross(Vector3 pOther) => new(
		Y * pOther.Z - Z * pOther.Y,
		Z * pOther.X - X * pOther.Z,
		X * pOther.Y - Y * pOther.X
	);
	
	public void EulerRotateX(float pDelta) {
		var rad = pDelta * MathF.PI / 180f;
		var (cos, sin) = (MathF.Cos(rad), MathF.Sin(rad));
		(Y, Z) = (cos * Y - sin * Z, cos * Z + sin * Y);
	}
	public void EulerRotateZ(float pDelta) {
		var rad = pDelta * MathF.PI / 180f;
		var (cos, sin) = (MathF.Cos(rad), MathF.Sin(rad));
		(X, Y) = (cos * X - sin * Y, cos * Y + sin * X);
	}
	public void EulerRotateY(float pDelta) {
		var rad = pDelta * MathF.PI / 180f;
		var (cos, sin) = (MathF.Cos(rad), MathF.Sin(rad));
		(X, Z) = (cos * X - sin * Z, cos * Z + sin * X);
	}
	public void Mapped(Func<float, float> pFunc) => (X, Y, Z) = (pFunc(X), pFunc(Y), pFunc(Z));
	public readonly Vector3 Map(Func<float, float> pFunc) => new(pFunc(X), pFunc(Y), pFunc(Z));
	public void SwapXY() => (X, Y) = (Y, X);
	public void SwapXZ() => (X, Z) = (Z, X);
	public void SwapYZ() => (Y, Z) = (Z, Y);
	public Vector3 SwapedXY() => new(Y, X, Z);
	public Vector3 SwapedXZ() => new(Z, Y, X);
	public Vector3 SwapedYZ() => new(X, Z, Y);
	public static Vector3 operator -(Vector3 lhs) => new(-lhs.X, -lhs.Y, -lhs.Z);
	public static Vector3 operator +(Vector3 lhs, Vector3 rhs) =>
		new(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
	public static Vector3 operator -(Vector3 lhs, Vector3 rhs) =>
		new(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
	public static Vector3 operator *(Vector3 lhs, Vector3 rhs) =>
		new(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);
	public static Vector3 operator /(Vector3 lhs, Vector3 rhs) =>
		new(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z);
	public static Vector3 operator +(float lhs, Vector3 rhs) =>
		new(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z);
	public static Vector3 operator +(Vector3 lhs, float rhs) =>
		new(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs);
	public static Vector3 operator -(float lhs, Vector3 rhs) =>
		new(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z);
	public static Vector3 operator -(Vector3 lhs, float rhs) =>
		new(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs);
	public static Vector3 operator *(float lhs, Vector3 rhs) =>
		new(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z);
	public static Vector3 operator *(Vector3 lhs, float rhs) =>
		new(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
	public static Vector3 operator /(float lhs, Vector3 rhs) =>
		new(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z);
	public static Vector3 operator /(Vector3 lhs, float rhs) =>
		new(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);

	public static bool operator ==(Vector3 lhs, Vector3 rhs) =>
		lhs.Equals(rhs);
	public static bool operator !=(Vector3 lhs, Vector3 rhs) => 
		!(lhs == rhs);
	
	public bool Equals(Vector3 pRhs) => 
		(X, Y, Z) == (pRhs.X, pRhs.Y, pRhs.Z);
	
	public override bool Equals( object? obj) =>
		obj is Vector3 rhs && Equals(rhs);

	public override int GetHashCode() =>
		HashCode.Combine(X, Y, Z);

#endregion

	public static Vector3 Parse(string s) {
		s = s.Replace('(', ' ');
		s = s.Replace(')', ' ');
		var split = s.Split(',');
		var (x, y, z) = (0f, 0f, 0f);
		float.TryParse(split[0], out x);
		if (split.Length >= 2) float.TryParse(split[1], out y);
		if (split.Length >= 3) float.TryParse(split[2], out z);
		return new(x,y,z);
	}
	public static bool TryParse(string? s, IFormatProvider? provider, out Vector3 result) {
		result = Zero; 
		if (s == null || string.IsNullOrWhiteSpace(s)) return false;
		s = s.Replace('(', ' ');
		s = s.Remove(')', ' ');
		var split = s.Split(',');
		
		var (x, y, z) = (0f, 0f, 0f);
		float.TryParse(split[0], out x);
		if (split.Length >= 2) float.TryParse(split[1], out y);
		if (split.Length >= 3) float.TryParse(split[2], out z);
		result = new(x,y,z);
		return true;
	}
}