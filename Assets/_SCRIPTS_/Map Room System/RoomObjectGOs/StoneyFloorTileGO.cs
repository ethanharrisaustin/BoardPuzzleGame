using UnityEngine;

namespace MapRooms
{
    public class StoneyFloorTileGO : FloorTileGO
    {
        [SerializeField] new Renderer renderer;
        [SerializeField] MeshFilter meshFilter;
        [SerializeField] Material[] materials;
        [SerializeField] Mesh[] meshes;

        public override void Spawn(RoomObject roomObject, RoomObject.FlySettings flySettings)
        {
            base.Spawn(roomObject, flySettings);

            int xPos = (int)GetPosition().x;
            int yPos = (int)GetPosition().z;

            int material =  Mathf.Abs((xPos + yPos) % materials.Length);

            renderer.material = materials[material];

            int rotation = (xPos + yPos * 4) % 4;

            switch(rotation)
            {
                case 1:
                    transform.Rotate(0, 90, 0);
                    break;
                case 2:
                    transform.Rotate(0, 180, 0);
                    break;
                case 3:
                    transform.Rotate(0, 270, 0);
                    break;
            }
            
            int _mesh = Mathf.Abs((xPos + (yPos * (meshes.Length - 1))) % meshes.Length);

            meshFilter.mesh = meshes[_mesh];
        }
    }
}