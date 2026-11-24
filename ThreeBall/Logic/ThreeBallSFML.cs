using System.Diagnostics;
using System.Text;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace ThreeBall;

public class ThreeBallSFML : ILogic {
	private List<Ball> _balls = new();
	private RenderWindow _renderWindow;
	
	public void Start() {
		
		_renderWindow = new RenderWindow(new VideoMode(new Vector2u(1920, 1080)), "Three body", Styles.Default, State.Windowed);
		_renderWindow.Closed += (pSender, pArgs) => _renderWindow.Close();

		var length = 2f;
		
		var hl = length / 2;
		var tan60 = (float)Math.Sqrt(3);
		
		_balls.Add(new(new(hl, -hl * tan60 / 3f), 1f));
		_balls.Add(new(new(-hl, -hl * tan60 / 3f), 1f));
		_balls.Add(new(new(0, hl * 2 * tan60 / 3f), 2f));
		
	}
	
	public void Update() {
		
		foreach (var ball1 in _balls) {
			foreach (var ball2 in _balls) {
				ball1.UpdateForce(ball2);
			}
		}
		
		_renderWindow.DispatchEvents();
		_renderWindow.Clear();

		Vector2 sum = Vector2.Zero;
		foreach (var ball in _balls) {
			ball.Update();
			ball.DrawOnSFML(_renderWindow);
			sum += ball.Pos;
		}

		var center = sum / _balls.Count;
		View view = new View(new Vector2f(center.X, center.Y), new(1920, 1080));
		_renderWindow.SetView(view);
			
		_renderWindow.Display();
		
	}

}