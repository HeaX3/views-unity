using Essentials.Meshes;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    [System.Serializable]
    public class CircularImage : Image
    {
        [SerializeField] [Range(0, 360)] private float _startAngle = -45;
        [SerializeField] [Range(0, 360)] private float _length = 90;
        [SerializeField] [Range(0, 1)] private float _circleWidth = 0.5f;
        [SerializeField] [Range(0, 0.1f)] private float _borderWidth = 0.1f;
        [SerializeField] [Range(2, 256)] private int _precision = 64;
        [SerializeField] private bool _mergeTips = true;
        [SerializeField] private bool _fillClockwise = true;

        /// <summary>
        /// Start of the part in euler angles
        /// </summary>
        public float StartAngle
        {
            get => _startAngle;
            set => _startAngle = value;
        }

        /// <summary>
        /// Length of the part in euler angles
        /// </summary>
        public float Length
        {
            get => _length;
            set => _length = value;
        }

        /// <summary>
        /// Width of the circle in % of the total radius. The cutout circle in the middle has a radius of
        /// <code>r(Inner Circle) = r(Outer Circle) - CircleWidth</code>
        /// </summary>
        public float CircleWidth
        {
            get => _circleWidth;
            set => _circleWidth = Mathf.Clamp01(value);
        }

        /// <summary>
        /// Width of the border in %
        /// </summary>
        public float BorderWidth
        {
            get => _borderWidth;
            set => _borderWidth = Mathf.Clamp(value, 0, 0.5f);
        }

        /// <summary>
        /// Merge tips when part fills full circle
        /// </summary>
        public bool MergeTips
        {
            get => _mergeTips;
            set => _mergeTips = value;
        }

        /// <summary>
        /// Fill clockwise or counter clockwise
        /// </summary>
        public bool FillClockwise
        {
            get => _fillClockwise;
            set => _fillClockwise = value;
        }

        /// <summary>
        /// Vertices around the circle
        /// </summary>
        public int Precision
        {
            get => _precision;
            set => _precision = value;
        }

        public override bool Raycast(Vector2 sp, Camera eventCamera)
        {
            var rectTransform = this.rectTransform;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out var p))
            {
                return false;
            }

            var rect = rectTransform.rect;
            var relativePoint = (p - rect.min) / rect.size;

            var center = new Vector2(0.5f, 0.5f);
            var handle = (relativePoint - center);
            var distanceFromCenter = (handle * 2).sqrMagnitude;
            var angle = Vector2.SignedAngle(handle, Vector2.up);
            if (angle < 0) angle = 360 + angle;

            return distanceFromCenter <= 1 && angle >= StartAngle && angle <= StartAngle + Length;
        }

        /// <summary>
        /// Callback function when a UI element needs to generate vertices.
        /// </summary>
        /// <param name="vh">VertexHelper utility.</param>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var mesh = GetMesh();
            vh.Clear();
            if (mesh == null) return;
            // Get data from mesh
            var verts = mesh.vertices;
            var uvs = mesh.uv;
            if (uvs.Length < verts.Length)
                uvs = new Vector2[verts.Length];
            // Get mesh bounds parameters
            Vector2 meshMin = mesh.bounds.min;
            Vector2 meshSize = mesh.bounds.size;
            var rectTransform = this.rectTransform;
            // Add scaled vertices
            for (var ii = 0; ii < verts.Length; ii++)
            {
                Vector2 v = (verts[ii] + Vector3.up + Vector3.right) / 2;
                //v.x = (v.x - meshMin.x) / meshSize.x;
                //v.y = (v.y - meshMin.y) / meshSize.y;
                v = Vector2.Scale(v - rectTransform.pivot, rectTransform.rect.size);
                vh.AddVert(v, color, uvs[ii]);
            }

            // Add triangles
            var tris = mesh.triangles;
            for (var ii = 0; ii < tris.Length; ii += 3)
                vh.AddTriangle(tris[ii], tris[ii + 1], tris[ii + 2]);
        }

        private MeshFragment GetMesh()
        {
            var totalSteps = _precision;
            var steps = Mathf.CeilToInt(_length / 360 * totalSteps);
            var step = _length / steps / 360;

            var vertices = new Vector3[(steps + 1) * 4]; //4 rows of vertices
            var uv = new Vector2[vertices.Length];
            var indices = new int[steps * 12]; //3 faces per step

            var radiusA = 1 - _circleWidth;
            var radiusB = 1 - (_circleWidth - _borderWidth * _circleWidth);
            var radiusC = 1 - (_borderWidth * _circleWidth);
            var radiusD = 1f;

            var relativeStartAngle = (_startAngle + 90) / 360;

            for (var i = 0; i <= steps; i++)
            {
                var angle = relativeStartAngle + i * step;
                var x = Mathf.Cos(angle * Mathf.PI * 2) * (_fillClockwise ? -1 : 1);
                var y = Mathf.Sin(angle * Mathf.PI * 2);

                var v = new Vector2(x, y);

                vertices[i * 4 + 0] = v * radiusA;
                vertices[i * 4 + 1] = v * radiusB;
                vertices[i * 4 + 2] = v * radiusC;
                vertices[i * 4 + 3] = v * radiusD;

                uv[i * 4 + 0] = new Vector2(i, 0f);
                uv[i * 4 + 1] = new Vector2(i, _borderWidth);
                uv[i * 4 + 2] = new Vector2(i, 1 - _borderWidth);
                uv[i * 4 + 3] = new Vector2(i, 1f);

                if (i >= steps) continue;
                indices[i * 12 + 0] = i * 4 + 0;
                indices[i * 12 + 1] = i * 4 + 1;
                indices[i * 12 + 2] = (i + 1) * 4 + 1;
                indices[i * 12 + 3] = (i + 1) * 4 + 0;

                indices[i * 12 + 4] = i * 4 + 1;
                indices[i * 12 + 5] = i * 4 + 2;
                indices[i * 12 + 6] = (i + 1) * 4 + 2;
                indices[i * 12 + 7] = (i + 1) * 4 + 1;

                indices[i * 12 + 8] = i * 4 + 2;
                indices[i * 12 + 9] = i * 4 + 3;
                indices[i * 12 + 10] = (i + 1) * 4 + 3;
                indices[i * 12 + 11] = (i + 1) * 4 + 2;
            }

            var result = new MeshFragment();
            result.vertices = vertices;
            result.uv = uv;
            result.SetIndices(indices, MeshTopology.Quads, null, 0);
            return result;
        }
    }
}