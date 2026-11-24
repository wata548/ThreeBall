using System.Diagnostics;
using ThreeBall;

public class Program {
	
	//==================================================|| Properties
	
	public static float TargetFrame { get; set; } = 150;
	public static int Term => (int)Math.Floor(1000f / TargetFrame);
	public static float DeltaTime { get; private set; } = 0;
	private static ILogic _logic;

	//==================================================|| Method	
	
	public static void Main() {
		var screen = new Screen(new(80, 80), new(12, 12));
		_logic = new ThreeBallMain(screen);
		
		if (_logic == null) 
			throw new("_logic isn't initialized. \nplease set _logic's value");
		
		_logic.Start();
		var watch = new Stopwatch();
		var elapsed = 0;
		while (true) {
			
			watch.Restart();
			try {
				_logic.Update();
			}
			catch(Exception e) {
				Console.Write(e);
				break;
			}
			var usedTime = (int)watch.ElapsedMilliseconds;
			watch.Stop();
			
			var term = Term;
			if (elapsed + usedTime <= term) {
				Thread.Sleep(term - (elapsed + usedTime));
				elapsed = 0;
				DeltaTime = term / 1000f;
			}
			else {
				elapsed += usedTime - term;
				DeltaTime = usedTime / 1000f;
			}
			Console.WriteLine($"Frame: {1f / DeltaTime}");
		}

		while (true) ;
	}
	
}