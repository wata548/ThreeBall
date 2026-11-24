using System.Text;

namespace ThreeBall;

public class ThreeBallMain(Screen pScreen): ILogic {
	private List<Ball> _balls = new();
	private Screen _screen = pScreen;
	private const float Margin = 2;
	
	public void Start() {
		var length = 2f;
		
		var hl = length / 2;
		var tan60 = (float)Math.Sqrt(3);
		_balls.Add(new(new(hl, -hl * tan60 / 3f), 1f) {
			Velocity = RandomDirection()
		});
		_balls.Add(new(new(-hl, -hl * tan60 / 3f), 1f) {
			Velocity = RandomDirection()
		});
		_balls.Add(new(new(0, hl * 2 * tan60 / 3f), 1f) {
			Velocity = RandomDirection()
		});
		

		Vector2 RandomDirection() {
			var r = new Random().NextSingle();
			return new(MathF.Cos(r), MathF.Sin(r));
		}
	}
	
	public void Update() {
		_screen.Init();
		
		var avg = _balls.Aggregate(Vector2.Zero, (s, b) => s + b.Pos) / _balls.Count;
		var t = 1 - MathF.Exp(-Program.DeltaTime);
		_screen.Pos = Vector2.Lerp(_screen.Pos, avg, t);
		_screen.CoordSize = Vector2.Lerp(_screen.CoordSize, GetRange(), t);
		
		foreach (var ball1 in _balls) {
			foreach (var ball2 in _balls) {
				ball1.UpdateForce(ball2);
			}
		}
		
		foreach (var ball in _balls) {
			ball.Update();
			_screen.Stamp(ball);
		}
		
		Print();
	}

	private void Print() {
		var result = _screen.Print();
		Console.SetCursorPosition(0, 0);
		Console.WriteLine(result);
	}

	private Vector2 GetRange() {
		var max = Vector2.One * float.MinValue;
		var min = Vector2.One * float.MaxValue;
		foreach (var ball in _balls) {
			max.X = float.Max(max.X, ball.Pos.X + ball.Radius);
			min.X = float.Min(min.X, ball.Pos.X - ball.Radius);
			max.Y = float.Max(max.Y, ball.Pos.Y + ball.Radius);
			min.Y = float.Min(min.Y, ball.Pos.Y - ball.Radius);
		}

		var temp = max - min + Vector2.One * (1 + Margin);
		var maxValue = float.Max(temp.X, temp.Y);
		return new(maxValue, maxValue);
	} 
}