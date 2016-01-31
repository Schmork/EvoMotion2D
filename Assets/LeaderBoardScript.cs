using UnityEngine;
using System.Linq;

namespace EvoMotion2D
{
    public class LeaderBoardScript : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            var cells = CellFactory.Cells;
            var orderedCells = cells.OrderBy(cell => cell.GetComponent<Cell.CellHandler>().CollectedMass).ToList();
            orderedCells.Reverse();

            var firstX = orderedCells.Take(40);

            var text = "";
            foreach (var cell in firstX)
            {
                var ch = cell.GetComponent<Cell.CellHandler>();
                var hexCol = hexFromFloat(ch.Color);
                text += "<color=" + hexCol + ">" + cell.name + ": " + ch.CollectedMass + "</color> \n";
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