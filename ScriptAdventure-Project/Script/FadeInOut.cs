using Godot;
using System;

public partial class FadeInOut : ColorRect
{
	private float time;
	private int count = 0;

	

	public void FadeIn(float value)
	{
		this.Visible = true;
		time = value;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "color", new Color(0,0,0,1), time);
		tween.TweenCallback(Callable.From(() => tween.Dispose()));

	}

	public void FadeOut(float value)
	{
		time = value;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "color", new Color(0,0,0,0), time);
		tween.TweenCallback(Callable.From(() => FinishFadeOut(tween)));
	}

	private void FinishFadeOut(Tween tween)
	{
		tween.Dispose();
		this.Visible = false;
	}

	public float GetOpacity()
	{
		return this.Color.A;
	}

}
