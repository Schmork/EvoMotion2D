using UnityEngine;
using System.Linq;

namespace EvoMotion2D
{
    public class LeaderBoardScript : MonoBehaviour
    {
		public int NumberOfEntries;		// ToDo: adjust UI output to really display the desired number of entries

        // Update is called once per frame
        void Update()
        {
            var cells = CellFactory.Cells;
			var maxCells = cells.OrderBy (cell => cell.GetComponent<Cell.CellHandler> ().CollectedMass).Reverse ().Take (NumberOfEntries);

            var text = "";
            foreach (var cell in maxCells)
            {
                var ch = cell.GetComponent<Cell.CellHandler>();
                var hexCol = hexFromFloat(ch.Color);
				var collectedMass = ch.CollectedMass.ToString("0.0");		// show the first digit only
                text += "<color=" + hexCol + ">" + cell.name + ": " + collectedMass + "</color> \n";
            }
            GetComponent<UnityEngine.UI.Text>().text = text;
        }

        string hexFromFloat(Color color)
        {
            return string.Format("#{0}{1}{2}",
                ((int)(color.r * 255)).ToString("X2"),
                ((int)(color.g * 255)).ToString("X2"),
                ((int)(color.b * 255)).ToString("X2"));
        }
    }
}