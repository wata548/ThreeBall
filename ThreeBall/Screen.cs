using System.Text;
using ColorMine.ColorSpaces;

namespace ThreeBall;

public record Vector2Int(int X, int Y) {
	public static Vector2 operator *(Vector2Int pLhs, Vector2 pRhs) =>
		new(pRhs.X * pLhs.X, pRhs.Y * pLhs.Y);
	public static Vector2 operator /(Vector2Int pLhs, Vector2 pRhs) =>
		new(pLhs.X / pRhs.X, pLhs.Y / pRhs.Y);
	public static Vector2 operator /(Vector2 pLhs, Vector2Int pRhs) =>
		new(pLhs.X / pRhs.X, pLhs.Y / pRhs.Y);
}

public class Screen {
	
	public readonly Vector2Int Size;
	public readonly Vector2Int HalfSize;
	public readonly Vector2 MinimumCoordSize;
	private readonly Ball?[] _buffer;
	public Vector2 CoordSize {
		get => field;
		set {
			if (value.X < MinimumCoordSize.X || value.Y < MinimumCoordSize.Y) 
				return;
			
			field = value;
			GridTerm = CoordSize / Size ;
			PixelCnt = Size / CoordSize ;
		}
	}
	
	public Vector2 Pos { get; set; }
	public Vector2 GridTerm { get; private set; }
	public Vector2 PixelCnt { get; private set; }

	public Screen(Vector2Int pSize, Vector2 pMinimumCoordSize) {
		Size = pSize;
		HalfSize = new(Size.X / 2 + Size.X % 2, Size.Y / 2 + Size.Y % 2);
		
		MinimumCoordSize = pMinimumCoordSize;
		CoordSize = pMinimumCoordSize;
		
		_buffer = Enumerable.Repeat<Ball?>(null, Size.X * Size.Y).ToArray();
	}
	
	public void Init() => Array.Fill(_buffer, null);
	
	public void Stamp(Ball pBall) {
		var temp = ((pBall.Pos - Pos) / GridTerm).Map(MathF.Ceiling);
		var screenPos = new Vector2Int((int)temp.X, (int)temp.Y);
		
		var circleSize = PixelCnt * pBall.Radius;
		var size = new Vector2Int((int)Math.Ceiling(circleSize.X), (int)Math.Ceiling(circleSize.Y));
		if (screenPos.X + size.X <= -HalfSize.X) return;
		if (screenPos.X - size.X >= HalfSize.X) return;
		if (screenPos.Y + size.Y <= -HalfSize.Y) return;
		if (screenPos.Y - size.Y >= HalfSize.Y) return;

		var sqTerm = GridTerm * GridTerm;
		for (int i = -size.Y; i <= size.Y; i++) {
			if(i + screenPos.Y <= -HalfSize.Y) continue;
			if (i + screenPos.Y >= HalfSize.Y) break;
			
			for (int j = -size.X; j <= size.X; j++) {
				if(j + screenPos.X <= -HalfSize.X) continue;
				if (j + screenPos.X >= HalfSize.X) break;
				
				if(MathF.Sqrt(i * i * sqTerm.Y + j * j * sqTerm.X) >= pBall.Radius) 
					continue;

				var idx = (screenPos.Y + i + HalfSize.Y) * Size.X + screenPos.X + j + HalfSize.X;
				_buffer[idx] = pBall;
			}
		}
	}

	public string Print() {
		var builder = new StringBuilder();
		builder.Append("\x1b[38;2;90;90;90m");
		
		IRgb v = new Rgb();
		var black = new Rgb() { R = 0, G = 0, B = 0 };
		
		var lastY = int.MinValue;
		for (int i = 0, num = 0; i < Size.X; i++) {
			var y = (int)MathF.Floor(GridTerm.Y * (i - HalfSize.Y) + Pos.Y);
			var yAxis = false;
			if (lastY != y) {
				lastY = y;
				yAxis = true;
			}
			
			var lastX = int.MinValue;
			for (int j = 0; j < Size.Y; j++, num++) {

				var context = "  ";

				var x = (int)MathF.Floor(GridTerm.X * (j - HalfSize.X) + Pos.X);
				var xAxis = false;
				if (lastX != x) {
					lastX = x;
					xAxis = true;
				}
				
				if (xAxis && yAxis) context = "++";
				else if (xAxis) context = "||";
				else if (yAxis) context = "--";
				
				if (_buffer[num] == null) {
					if (v != black) {
						v = black;
						builder.Append("\x1b[48;2;0;0;0m");
					}
				}
				else {
					var c = _buffer[num]!.Color;
					if (v != c) {
						v = c;
						builder.Append($"\x1b[48;2;{(int)c.R};{(int)c.G};{(int)c.B}m");
					}
				}
				builder.Append(context);
			}

			if (v != black) {
				v = black;
				builder.Append("\x1b[48;2;0;0;0m");
			}
			builder.AppendLine("|");
		}

		return builder.ToString();
	}  
}