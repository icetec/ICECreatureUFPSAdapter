// ##############################################################################
//
// ICECreatureUFPSAdapter.cs
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
	/*
	[System.Serializable]
	public class ICECreatureUFPSCreatureBodyPartObject : System.Object 
	{
		public string BodyPart = "";
		public float DamageDistance = 1;
		public float DamageMultiplier = 1;
	}*/

	[System.Serializable]
	public class ICECreatureUFPSCreatureDamageObject : System.Object 
	{
		public ICECreatureUFPSCreatureDamageObject(){}
		public ICECreatureUFPSCreatureDamageObject( vp_DamageInfo.DamageType _damage_type )
		{
			DamageType = _damage_type;
		}

		//public List<ICECreatureUFPSCreatureBodyPartObject> BodyParts = new List<ICECreatureUFPSCreatureBodyPartObject>();

		public bool UseAdvanced = false;
		public float InfluenceDamage = 1.5f;
		public float InfluenceStress = 0.5f;
		public float InfluenceDebility = 0.5f;
		public float InfluenceHunger = 0;
		public float InfluenceThirst = 0;
		public string BehaviourModeKey = "";
		public vp_DamageInfo.DamageType DamageType = vp_DamageInfo.DamageType.Bullet;
	}

	[System.Serializable]
	public class ICECreatureUFPSPlayerDamageObject : System.Object 
	{
		public ICECreatureUFPSPlayerDamageObject(){}
		public ICECreatureUFPSPlayerDamageObject( string _behaviour )
		{
			DamageBehaviourModeKey = _behaviour;
		}

		public string DamageBehaviourModeKey = "";
		public float Damage = 1;
		public float DamageRange = 2;
		public float DamageInterval = 0.5f;
		
		public vp_DamageInfo.DamageType DamageType = vp_DamageInfo.DamageType.Bullet;
		
		public bool RequireDamageHandler = true;
		public string DamageMethodName = "Damage";
	}

	[RequireComponent (typeof (ICECreatureControl))]
	public class ICECreatureUFPSAdapter : vp_DamageHandler 
	{

		private ICECreatureControl m_Controller = null;

		public List<ICECreatureUFPSCreatureDamageObject> CreatureDamages = new List<ICECreatureUFPSCreatureDamageObject>();
		public List<ICECreatureUFPSPlayerDamageObject> PlayerDamages = new List<ICECreatureUFPSPlayerDamageObject>();

		public bool UseCreatureDamage = true;
		public bool UseMultipleCreatureDamageHandler = false;
		public bool UseAdvanced = false;
		public float InfluenceDamage = 1.5f;
		public float InfluenceStress = 0.5f;
		public float InfluenceDebility = 0.5f;
		public float InfluenceHunger = 0;
		public float InfluenceThirst = 0;
		public string BehaviourModeKey = "";

		public bool UseMultiplePlayerDamageHandler = false;
		public bool UsePlayerDamage = false;
		private GameObject m_Player = null;
		public GameObject Player{
			get{ return m_Player = ( m_Player == null ? GameObject.FindGameObjectWithTag("Player") : m_Player ); }
		}
		private vp_DamageHandler m_PlayerDamageHandler = null;
		public vp_DamageHandler PlayerDamageHandler{
			get{ return m_PlayerDamageHandler = ( m_PlayerDamageHandler == null ? (Player != null ? Player.transform.GetComponent<vp_DamageHandler>():null ) : m_PlayerDamageHandler ); }
		}
		public string PlayerDamageBehaviourModeKey = "";
		public float PlayerDamage = 1;
		public float PlayerDamageRange = 2;
		private float m_PlayerDamageTimer = 0;
		public float PlayerDamageInterval = 0.5f;

		public vp_DamageInfo.DamageType PlayerDamageType = vp_DamageInfo.DamageType.Bullet;

		public bool RequireDamageHandler = true;
		public string PlayerDamageMethodName = "Damage";

		protected new void Awake ()
		{
			m_Controller = GetComponent<ICECreatureControl>();

			m_Player = GameObject.FindGameObjectWithTag("Player");

			if( m_Player != null )
				m_PlayerDamageHandler = Player.transform.GetComponent<vp_DamageHandler>();

		}

		public override void Die()
		{
			// do nothing, will be already handled by the control
		}

		public bool AddCreatureDamage( vp_DamageInfo.DamageType _damage_type )
		{
			ICECreatureUFPSCreatureDamageObject _damage = GetCreatureDamage( _damage_type );

			if( _damage != null )
				return false;

			CreatureDamages.Add( new ICECreatureUFPSCreatureDamageObject( _damage_type ) );
			return true;
		}

		private ICECreatureUFPSCreatureDamageObject GetCreatureDamage( vp_DamageInfo.DamageType _damage_type )
		{
			foreach( ICECreatureUFPSCreatureDamageObject _damage in CreatureDamages ){
				if( _damage != null && _damage.DamageType == _damage_type )
					return _damage;
			}
			return null;
		}

		public override void Damage(float damage)
		{
			Damage(new vp_DamageInfo(damage, null));
		}

		public override void Damage( vp_DamageInfo damageInfo )
		{
			if( ! UseCreatureDamage )
				return;

			m_Controller = GetComponent<ICECreatureControl>();

			if( m_Controller != null )
			{
				//vp_HitscanBullet[] _hits = transform.GetComponentsInChildren<vp_HitscanBullet>();

			//	Debug.Log( _hits.Length );

				if( UseMultipleCreatureDamageHandler && CreatureDamages.Count > 0 )
				{
					ICECreatureUFPSCreatureDamageObject _damage = GetCreatureDamage( damageInfo.Type );
					if( _damage != null )
					{
						if( _damage.UseAdvanced )
						{
							m_Controller.Creature.Status.AddDamage( _damage.InfluenceDamage );
							m_Controller.Creature.Status.AddStress( _damage.InfluenceStress );
							m_Controller.Creature.Status.AddDebility( _damage.InfluenceDebility );
							m_Controller.Creature.Status.AddHunger( _damage.InfluenceHunger );
							m_Controller.Creature.Status.AddThirst( _damage.InfluenceThirst );
						}
						else
						{
							m_Controller.Creature.Status.AddDamage( damageInfo.Damage );
						}

						if( _damage.BehaviourModeKey != "" && m_Controller.Creature.Status.IsDead == false )
							m_Controller.Creature.Behaviour.SetBehaviourModeByKey( _damage.BehaviourModeKey );
					}
				}
				else
				{
					if( UseAdvanced )
					{
						m_Controller.Creature.Status.AddDamage( InfluenceDamage );
						m_Controller.Creature.Status.AddStress( InfluenceStress );
						m_Controller.Creature.Status.AddDebility( InfluenceDebility );
						m_Controller.Creature.Status.AddHunger( InfluenceHunger );
						m_Controller.Creature.Status.AddThirst( InfluenceThirst );
					}
					else
					{
						m_Controller.Creature.Status.AddDamage( damageInfo.Damage );
					}
					
					if( BehaviourModeKey != "" && m_Controller.Creature.Status.IsDead == false )
						m_Controller.Creature.Behaviour.SetBehaviourModeByKey( BehaviourModeKey );
				}
			}

			if (!enabled)
				return;
			
			if (!vp_Utility.IsActive(gameObject))
				return;
			
			// damage is always done in singleplayer, but only in multiplayer if you are the master
			if (!vp_Gameplay.IsMaster)
				return;
			
			if (CurrentHealth <= 0.0f)
				return;
			
			if (damageInfo != null)
			{
				if (damageInfo.Source != null)
					Source = damageInfo.Source;
				if (damageInfo.OriginalSource != null)
					OriginalSource = damageInfo.OriginalSource;

				Debug.Log("Damage! Source: " + damageInfo.Source + " ... " + "OriginalSource: " + damageInfo.OriginalSource);
			}
			
			// if we somehow shot ourselves with a bullet, ignore it
			if ((damageInfo.Type == vp_DamageInfo.DamageType.Bullet) && (m_Source == Transform))
				return;
			
			// subtract damage from health
			CurrentHealth = Mathf.Min(CurrentHealth - damageInfo.Damage, MaxHealth);
			
			// in multiplayer, report damage for score tracking purposes
			if (vp_Gameplay.IsMultiplayer && (damageInfo.Source != null))
				vp_GlobalEvent<Transform, Transform, float>.Send("TransmitDamage", Transform.root, damageInfo.OriginalSource, damageInfo.Damage);
			
			// detect and transmit death as event
			if (CurrentHealth <= 0.0f)
			{
				// send the 'Die' message, to be picked up by vp_DamageHandlers and vp_Respawners
				if (m_InstaKill)
					SendMessage("Die");
				else
					vp_Timer.In(UnityEngine.Random.Range(MinDeathDelay, MaxDeathDelay), delegate() { SendMessage("Die"); });
			}
		}

		public bool AddPlayerDamage( string _behaviour )
		{
			if( _behaviour.Trim() == "" )
				return false;

			PlayerDamages.Add( new ICECreatureUFPSPlayerDamageObject( _behaviour ) );
			return true;
		}

		private ICECreatureUFPSPlayerDamageObject GetPlayerDamageByBehaviour( string _behaviour )
		{
			foreach( ICECreatureUFPSPlayerDamageObject _damage in PlayerDamages )
			{
				if( UsePlayerDamage && _damage.DamageBehaviourModeKey != "" && _damage.DamageBehaviourModeKey == _behaviour )
					return _damage;
			}
			return null;
		}

		private ICECreatureUFPSPlayerDamageObject GetPlayerDamage()
		{
			string _behaviour = m_Controller.Creature.Behaviour.BehaviourModeKey;
			float _distance = Vector3.Distance( Player.transform.position, transform.position );
			List<ICECreatureUFPSPlayerDamageObject> _damages = new List<ICECreatureUFPSPlayerDamageObject>();
			foreach( ICECreatureUFPSPlayerDamageObject _damage in PlayerDamages )
			{
				if( UsePlayerDamage && _damage.DamageBehaviourModeKey != "" && _damage.DamageBehaviourModeKey == _behaviour && _distance <= _damage.DamageRange )
					_damages.Add( _damage );
			}

			if( _damages.Count == 1 ){
				return _damages[0];
			}else if( _damages.Count > 1 ){
				return _damages[Random.Range(0,_damages.Count)];
			}else
				return null;
		}

		public void Update ()
		{
			if( ! UsePlayerDamage )
				return;

			if( PlayerDamages.Count > 0 )
			{
				ICECreatureUFPSPlayerDamageObject _damage = GetPlayerDamage();

				if( _damage != null )
				{
					m_PlayerDamageTimer += Time.deltaTime;
					if( m_PlayerDamageTimer >= _damage.DamageInterval )
					{
						if( ( PlayerDamageHandler != null ) && ( Source != null ) )
							PlayerDamageHandler.Damage( new vp_DamageInfo( _damage.Damage, Source, _damage.DamageType ));
						else if( ! RequireDamageHandler )
							Player.SendMessage( _damage.DamageMethodName, _damage.Damage, SendMessageOptions.DontRequireReceiver);
						
						m_PlayerDamageTimer = 0;
					}
				}
				else
					m_PlayerDamageTimer = 0;
			}
			else
			{
				if( PlayerDamageBehaviourModeKey != "" && PlayerDamageBehaviourModeKey == m_Controller.Creature.Behaviour.BehaviourModeKey )
				{
					float _distance = Vector3.Distance( Player.transform.position, transform.position );
					if( _distance <= PlayerDamageRange )
					{
						m_PlayerDamageTimer += Time.deltaTime;
						if( m_PlayerDamageTimer >= PlayerDamageInterval )
						{
							if( ( PlayerDamageHandler != null ) && ( Source != null ) )
								PlayerDamageHandler.Damage( new vp_DamageInfo( PlayerDamage, Source, PlayerDamageType ));
							else if( ! RequireDamageHandler )
								Player.SendMessage( PlayerDamageMethodName, PlayerDamage, SendMessageOptions.DontRequireReceiver);

							m_PlayerDamageTimer = 0;
						}
					}
				}
				else
					m_PlayerDamageTimer = 0;
			}
		}
	}
}
