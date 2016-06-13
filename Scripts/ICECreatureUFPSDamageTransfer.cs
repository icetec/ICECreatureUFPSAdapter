// ##############################################################################
//
// ICECreatureUFPSDamageTransfer.cs
// Version 1.1.2
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

namespace ICE.Creatures.Adapter
{
	public class ICECreatureUFPSDamageTransfer : vp_DamageTransfer {

		public float DamageMultiplier = 10;
		public float DamageMultiplierMaximum = 100;

		/// <summary>
		/// forwards damage in UFPS format to a damagehandler on the target object
		/// </summary>
		public override void Damage(vp_DamageInfo damageInfo)
		{
			damageInfo.Damage = damageInfo.Damage * DamageMultiplier;
				
			base.Damage( damageInfo );
		}


		/// <summary>
		/// forwards damage in float format by executing the method 'Damage(float)'
		/// on the target object
		/// </summary>
		public override void Damage(float damage){
			base.Damage( damage * DamageMultiplier );
		}
	}
}
