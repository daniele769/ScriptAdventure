using Godot;
using System.Collections.Generic;
using MEC;
using System.Runtime.CompilerServices;
using System.Linq;
using static Godot.Node;
using System;

public static class Static_NodeMethod 
{
	////////////////////////Node_Tree////////////////////////////

	public static Node GetChildType(Node parent, string type)
	{
		int n = parent.GetChildCount();
		for (int i = 0; i < n; i++)
		{
			if (parent.GetChild(i).GetClass().Equals(type))
			{
				return parent.GetChild(i);
			}
		}
		return null;
	}

	public static Node GetInternalChildType(Node parent, string type)
	{
		Node child;
		int n = parent.GetChildCount();
		for (int i = 0; i < n; i++)
		{
			if (parent.GetChild(i).GetClass().Equals(type))
			{
				child = parent.GetChild(i);
				return child;
			}

			if(parent.GetChild(i).GetChildCount() > 0){
				child = parent.GetChild(i);
				child = GetInternalChildType(child, type);

				if(child != null)
					return child;
			}
		}
		return null;
	}

	public static Node GetChildWhitName(Node parent, string name)
	{
		int n = parent.GetChildCount();
		for (int i = 0; i < n; i++)
		{
			if (parent.GetChild(i).Name.Equals(name))
			{
				return parent.GetChild(i);
			}
			
		}
		return null;
	}

	public static Node GetInternalChildName(Node parent, string name)
	{
		Node child;
		int n = parent.GetChildCount();
		for (int i = 0; i < n; i++)
		{
			if (parent.GetChild(i).Name.Equals(name))
			{
				child = parent.GetChild(i);
				return child;
			}

			if(parent.GetChild(i).GetChildCount() > 0){
				//GD.Print("Checking internal child");
				child = parent.GetChild(i);
				child = GetInternalChildName(child, name);

				if(child != null){
					//GD.Print("Find Child!");
					return child;
				}
			}
		}
		return null;
	}

	public static Node GetParentWithName(Node child, string name){
		Node parent;
		if(child.GetParent() != null)
			parent = child.GetParent();
		else
			return null;

		if(parent.Name.Equals(name)){
			return parent;
		}
		parent = GetParentWithName(parent, name);
		return parent;
			
	}

	public static Node GetParentWithType(Node child, string type){
		Node parent;
		if(child.GetParent() != null)
			parent = child.GetParent();
		else
			return null;

		if(parent.GetClass().Equals(type)){
			return parent;
		}
		parent = GetParentWithType(parent, type);
		return parent;
			
	}

	public static Node DuplicateNodeInternal(Node node)
	{
		Node duplicated = node.Duplicate((int) DuplicateFlags.Scripts);
		DuplicateChild(node, duplicated);
		return duplicated;
		
	}

	private static void DuplicateChild(Node node, Node duplicated)
	{
		int childCount = node.GetChildCount();
		//GD.Print("Numero figli = " + childCount);
		for(int i = 0; i < childCount; i++){
			Node child = node.GetChild(i);
			Node childDuplicated = child.Duplicate((int) DuplicateFlags.Scripts);
			//GD.Print("childDuplicated added = " + childDuplicated);
			duplicated.AddChild(childDuplicated);
			DuplicateChild(child, childDuplicated);
		}
	}

	public static int CalcPosInParentChildreen(Node parent, Node child){
		for(int i = 0; i < parent.GetChildCount(); i++){
			if(parent.GetChild(i) == child)
				return i;
		}
		return -1;
	}

	public static List<Node> GetAllChild(List<Node> lista, Node root){
		int childCount = root.GetChildCount();
		if(childCount == 0)
			return lista;
		for(int i = 0; i < childCount; i++){
			lista.Add(root.GetChild(i));
			GetAllChild(lista, root.GetChild(i));
		}
		return lista;
	}

	public static List<T> GetAllChildWithTypeInList<T>(List<Node> list, string type)
    {
        List<T> typeList = new List<T>();
		
        foreach(Node node in list)
        {
            if(node.GetClass().Equals(type))
            {
				//GD.Print("CheckType entered");
                typeList.Add((T)Convert.ChangeType(node, (typeof(T))));
            }
        }
        return typeList;
    }
}
