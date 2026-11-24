using ColorMine.ColorSpaces;
using SFML.Graphics;

namespace ThreeBall;

public class Ball(Vector2 pPos, float pMass) {
	public const float Gravity = 9.8f * 1.5f;
	public readonly float Mass = pMass;
	public readonly float Radius = 1;
	public IRgb Color { get; private set; }
	public Vector2 Pos { get; set; } = pPos;
	public Vector2 Velocity { get; set; }
	private Vector2 _force;

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
		Color = new Hsv(){H = Velocity.Distance * 7, S = 1, V = 1}.ToRgb();
	}

	public void DrawOnSFML(IRenderTarget rt) {
		const float v = 30;
		var circle = new CircleShape(v, 32);
		circle.Origin = new(Pos.X * v, Pos.Y * v);
		
		rt.Draw(circle);
		
	}
	
}