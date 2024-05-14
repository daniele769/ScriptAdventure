using Godot;
using MEC;
using System;
using System.Collections.Generic;

public static class Static_AnimationMethod 
{
    public static IEnumerator<double> MakeAnimation(Sprite2D sprite, int coefficienteStart, int coefficienteEnd, float time = 0.12f)
	{
		if (coefficienteStart < coefficienteEnd)
		{
			for (int i = coefficienteStart; i < coefficienteEnd; i++)
			{
				sprite.Frame = i;
				yield return Timing.WaitForSeconds(time);
			}
		}
		else
		if (coefficienteStart > coefficienteEnd)
		{
			for (int i = coefficienteStart - 1; i > coefficienteEnd - 1; i--)
			{
				sprite.Frame = i;
				yield return Timing.WaitForSeconds(time);
			}
		}
	}

	public static IEnumerator<double> MakeAnimation(Sprite2D sprite, int coefficienteStart, int coefficienteEnd, Animable obj, float time = 0.12f)
	{
		if (coefficienteStart < coefficienteEnd)
		{
			for (int i = coefficienteStart; i < coefficienteEnd; i++)
			{
				obj.Set_IsPlaying(true);
				sprite.Frame = i;
				yield return Timing.WaitForSeconds(time);
			}
		}
		else
		if (coefficienteStart > coefficienteEnd)
		{
			for (int i = coefficienteStart - 1; i > coefficienteEnd - 1; i--)
			{
				obj.Set_IsPlaying(true);
				sprite.Frame = i;
				yield return Timing.WaitForSeconds(time);
			}
		}
		obj.Set_IsPlaying(false);
	}
}
