﻿using UnityEngine;

namespace EvoMotion2D.Cell
{
	public class Shrinker : MonoBehaviour
	{
		[Range(0.001f, 1)]
		public float MinMass;
		public static float StaticMinMass;

        [Range(0.0000001f, 0.0001f)]
        public float ConstantLoss;
        
        [Range(0.0000001f, 0.0001f)]
        public float RelativeLoss;

        CellHandler ch;

		// Use this for initialization
		void Start ()
		{
			StaticMinMass = MinMass;
			ch = GetComponent<CellHandler> ();
		}
	
		// Update is called once per frame
		void Update ()
		{            
			if (ch.Age > ch.MaxAge) {
				var overAge = ch.Age - ch.MaxAge;
				
				ch.Mass *= 1 - RelativeLoss * overAge;
				ch.Mass -= ConstantLoss * overAge;
			}

            if (ch.Mass < MinMass)
            {
                CellFactory.Cells.Remove(gameObject);
                Destroy(gameObject.GetComponentInChildren<CellHandler>().toolTip.gameObject);
                Destroy(gameObject);
            }
		}
	}
}
