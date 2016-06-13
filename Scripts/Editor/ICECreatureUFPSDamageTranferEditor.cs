// ##############################################################################
//
// ICECreatureUFPSAdapterEditor.cs
// Version 1.1
//
// © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.ice-technologies.com
// mailto:support@ice-technologies.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;

using ICE.Creatures;
using ICE.Creatures.EditorUtilities;

namespace ICE.Creatures.Adapter
{
	[CustomEditor(typeof(ICECreatureUFPSDamageTransfer))]
	public class ICECreatureUFPSDamageTranferEditor : ICEWorldBehaviourEditor
	{
		public override void OnInspectorGUI()
		{
			ICECreatureUFPSDamageTransfer _target = DrawMonoHeader<ICECreatureUFPSDamageTransfer>();
			DrawCreatureUFPSDamageTransfer( _target );
			DrawMonoFooter( _target );
		}

		public void DrawCreatureUFPSDamageTransfer( ICECreatureUFPSDamageTransfer _target )
		{
			_target.DamageMultiplier = ICEEditorLayout.MaxDefaultSlider( "Damage Multiplier", "Increases or decreases the damage according the body part", _target.DamageMultiplier, 0.25f, - _target.DamageMultiplierMaximum, ref _target.DamageMultiplierMaximum, 1, "" );
		
			_target.TargetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", _target.TargetObject, typeof(GameObject), true);
		}
	}
}
