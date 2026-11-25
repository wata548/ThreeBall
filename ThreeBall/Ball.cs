using ColorMine.ColorSpaces;
using SFML.Graphics;

namespace ThreeBall;

public class Ball(Vector2 pPos, float pMass) {
	public const float TrailTerm = 0.1f;
	public const int TrailMaxCnt = 10;
	
	public const float Gravity = 9.8f * 1.5f;
	public readonly float Mass = pMass;
	public readonly float Radius = 1;

	public Screen.CircleData CircleData => new(Pos, Color, Radius);
	public IRgb Color { get; private set; }
	public Vector2 Pos { get; set; } = pPos;
	public Vector2 Velocity { get; set; }
	private Vector2 _force;

	private float _trailChecker = 0f;
	private record TrailData(Vector2 Pos, IRgb Color);
	private Queue<TrailData> _trailRecord = new();
		
	public void UpdateForce(Ball pBall) {
		if (pBall == this) return;
		var direction = pBall.Pos - Pos;
		var dist = direction.Distance;
		direction /= dist;
		_force += direction * (Gravity * (Mass * pBall.Mass) / dist);
	} 
	
	public void Update() {
		Pos += Velocity * Program.DeltaTime;
		Velocity += Program.DeltaTime * _force;
		_force = Vector2.Zero;
		var hue = Velocity.Distance * 25;
		//const float ColorCount = 30;
		//hue = MathF.Round(hue / 360 * ColorCount) / ColorCount * 360;
		hue = 270 - hue;
		
		Color = new Hsv{H = hue, S = 1, V = 1}.ToRgb();

		_trailChecker += Program.DeltaTime;
		if (_trailChecker < TrailTerm)
			return;

		_trailChecker -= TrailTerm;
		if (_trailRecord.Count == TrailMaxCnt)
			_trailRecord.Dequeue();
		_trailRecord.Enqueue(new(Pos, Color));
	}

	public IEnumerable<Screen.CircleData> GetTrailData() {
		var cnt = _trailRecord.Count;
		var term = Radius / cnt;
		var num = 0f;
		foreach (var data in _trailRecord) {
			yield return new(data.Pos, data.Color, num += term);
		}
	}

	public void DrawOnSFML(IRenderTarget rt) {
		const float v = 30;
		var circle = new CircleShape(v, 32);
		circle.Origin = new(Pos.X * v, Pos.Y * v);
		
		rt.Draw(circle);
		
	}
	
}