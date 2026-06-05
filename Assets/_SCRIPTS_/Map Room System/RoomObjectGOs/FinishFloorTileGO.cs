using Cardboard;
using UnityEngine;

namespace MapRooms
{
    public class FinishFloorTileGO : LightUpFloorTileGO
    {
        [SerializeField] int numberPieces = 1;

        public static FinishFloorTileGO FinishTile { get; private set; }

        public override void Spawn(RoomObject roomObject, RoomObject.FlySettings flySettings)
        {
            base.Spawn(roomObject, flySettings);

            FinishTile = this;
        }

        protected override bool OnObjectEnter(Collider other)
        {
            if (UI_CompletionMenu.isOpen || UI_CompletionMenu.CannotOpen()) return false;
            
            if (!base.OnObjectEnter(other)) return false;

            if (!GetObjectsOnTile<CardboardHolderGO>(out var cardboardHolders)) return true;

            for (int i = 0; i < cardboardHolders.Length; ++i)
            {
                if (!cardboardHolders[i].ContainsPlayerCharacter()) continue;

                cardboardHolders[i].ShrinkCardboardHolder();
            }

            bool win = cardboardHolders.Length >= numberPieces;

            if (win)
            {
                UI_CompletionMenu.instance.Open();
            }

            return true;
        }

        public override void SetValues(string[] values)
        {
            base.SetValues(values);

            int.TryParse(values[values.Length - 1], out numberPieces);
        }

        public override void GetValues(out string[] values)
        {
            base.GetValues(out var baseValues);

            values = new string[baseValues.Length + 1];

            values[baseValues.Length] = numberPieces.ToString();
        }
    }
}