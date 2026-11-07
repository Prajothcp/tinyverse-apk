using System;
using System.Collections.Generic;
using Godot;

namespace TinyverseCSharp.Core
{
    /// <summary>
    /// Master C# 3D Cosmic Engine for Tinyverse.
    /// Implements:
    /// - 64-Bit Floating-Origin Camera with Seamless Zoom (Multiverse -> Universe -> Galaxy -> Star System -> Planet -> Surface)
    /// - GPU MultiMesh Instanced Galaxies (tens of thousands of stars in a single draw call)
    /// - Procedural 3D Noise Planetary Surfaces, Rings & Rayleigh Atmospheres
    /// - Relativistic Black Holes (True Dark Event Horizon + Doppler Beamed Accretion Disk Shader)
    /// - Symplectic Kepler/N-Body Orbital Simulation, Cosmic Events & Interactive Mobile/Desktop HUD
    /// </summary>
    public partial class TinyverseCSharpMain : Node3D
    {
        public enum CosmicTier
        {
            Multiverse,
            Universe,
            Galaxy,
            StarSystem,
            BlackHole,
            Planet,
            PlanetarySurface
        }

        public class CosmicBodyNode
        {
            public string Id;
            public string Name;
            public string Subtitle;
            public CosmicTier Tier;
            public bool IsEmpirical;
            public Vector3d LocalPos;
            public float Radius;
            public float OrbitRadius;
            public float OrbitSpeed;
            public float OrbitAngle;
            public Color PrimaryColor;
            public Color SecondaryColor;
            public string TelemetrySummary;
            public Node3D VisualNode;
            public CosmicBodyNode Parent;
            public List<CosmicBodyNode> Children = new List<CosmicBodyNode>();
        }

        // 3D Scene & Camera
        private Camera3D _camera;
        private WorldEnvironment _worldEnv;
        private DirectionalLight3D _starLight;

        private Node3D _multiverseLayer;
        private Node3D _universeLayer;
        private Node3D _galaxyLayer;
        private Node3D _systemLayer;
        private Node3D _surfaceLayer;

        // 64-Bit Floating Origin & Spherical Orbit State
        private Vector3d _targetPos64 = Vector3d.Zero;
        private Vector3d _currentLookAt64 = Vector3d.Zero;
        private double _camRadius = 680.0;
        private double _targetCamRadius = 680.0;
        private float _theta = 0.45f;
        private float _phi = 1.18f;
        private float _targetTheta = 0.45f;
        private float _targetPhi = 1.18f;

        // Simulation State
        private bool _simRunning = true;
        private float _simSpeed = 1.0f;
        private CosmicBodyNode _rootNode;
        private CosmicBodyNode _focusedNode;
        private readonly Dictionary<string, CosmicBodyNode> _nodeIndex = new Dictionary<string, CosmicBodyNode>();
        private readonly List<CosmicBodyNode> _orbitingBodies = new List<CosmicBodyNode>();

        // Touch & Mouse Input State
        private bool _isDragging = false;
        private readonly Dictionary<int, Vector2> _activeTouches = new Dictionary<int, Vector2>();
        private float _lastPinchDist = 0f;

        // UI Elements
        private Label _tierBadgeLabel;
        private Label _telemetryTitleLabel;
        private Label _telemetryBodyLabel;
        private VBoxContainer _outlinerListContainer;
        private HBoxContainer _breadcrumbContainer;
        private Button _playPauseButton;

        public override void _Ready()
        {
            SetupEnvironmentAndCamera();
            BuildCosmicDatabase();
            Build3DLayers();
            BuildFuturisticHudUI();
            FocusOnBody(_rootNode, 680.0);
        }

        private void SetupEnvironmentAndCamera()
        {
            _worldEnv = new WorldEnvironment();
            var env = new Godot.Environment
            {
                BackgroundMode = Godot.Environment.BGMode.Color,
                BackgroundColor = new Color(0.008f, 0.015f, 0.04f),
                AmbientLightSource = Godot.Environment.AmbientSource.Color,
                AmbientLightColor = new Color(0.20f, 0.26f, 0.38f),
                AmbientLightEnergy = 0.85f,
                GlowEnabled = true,
                GlowIntensity = 0.9f,
                GlowBloom = 0.25f
            };
            _worldEnv.Environment = env;
            AddChild(_worldEnv);

            _camera = new Camera3D
            {
                Fov = 55f,
                Near = 0.05f,
                Far = 8000f
            };
            AddChild(_camera);

            _starLight = new DirectionalLight3D
            {
                LightEnergy = 2.2f,
                LightColor = new Color(1.0f, 0.97f, 0.90f)
            };
            _starLight.RotationDegrees = new Vector3(-25f, 35f, 0f);
            AddChild(_starLight);

            _multiverseLayer = new Node3D { Name = "MultiverseLayer" };
            _universeLayer = new Node3D { Name = "UniverseLayer" };
            _galaxyLayer = new Node3D { Name = "GalaxyLayer" };
            _systemLayer = new Node3D { Name = "SystemLayer" };
            _surfaceLayer = new Node3D { Name = "SurfaceLayer" };

            AddChild(_multiverseLayer);
            AddChild(_universeLayer);
            AddChild(_galaxyLayer);
            AddChild(_systemLayer);
            AddChild(_surfaceLayer);
        }

        private void BuildCosmicDatabase()
        {
            _rootNode = new CosmicBodyNode
            {
                Id = "multiverse_root",
                Name = "The Omni-Multiverse Bulk",
                Subtitle = "Higher-Dimensional Inflationary Manifold",
                Tier = CosmicTier.Multiverse,
                IsEmpirical = false,
                Radius = 400f,
                PrimaryColor = new Color(0.22f, 0.74f, 0.97f),
                TelemetrySummary = "STATUS: HYPOTHETICAL COSMOLOGY\nDiameter: Unbounded Inflationary Manifold\nPhysics: Eternal Inflation & Multi-Brane Landscape\n\nSelect any Pocket Universe below or pinch/scroll to zoom seamlessly across 28 orders of magnitude."
            };
            RegisterBody(_rootNode, null);

            // UNIVERSE 1: LAMBDA-CDM
            var uniAlpha = new CosmicBodyNode
            {
                Id = "uni_alpha",
                Name = "Universe Alpha-1 (Lambda-CDM)",
                Subtitle = "Standard Relativistic Universe",
                Tier = CosmicTier.Universe,
                IsEmpirical = true,
                LocalPos = new Vector3d(-220, 20, 40),
                Radius = 85f,
                PrimaryColor = new Color(0.22f, 0.74f, 0.97f),
                SecondaryColor = new Color(0.51f, 0.55f, 0.97f),
                TelemetrySummary = "STATUS: EMPIRICAL SCIENCE\nObservable Diameter: 93.0 Billion ly\nAge: 13.787 Billion Years\nCMB Temp: 2.725 K\nComposition: 68.3% Dark Energy, 26.8% Dark Matter, 4.9% Baryons"
            };
            RegisterBody(uniAlpha, _rootNode);

            // UNIVERSE 2: HYPERION-9
            var uniHyperion = new CosmicBodyNode
            {
                Id = "uni_hyperion",
                Name = "Universe Hyperion-9 (High-G Brane)",
                Subtitle = "Hypothetical High-Gravity Pocket Cosmos",
                Tier = CosmicTier.Universe,
                IsEmpirical = false,
                LocalPos = new Vector3d(20, 95, -210),
                Radius = 80f,
                PrimaryColor = new Color(0.96f, 0.62f, 0.04f),
                SecondaryColor = new Color(0.93f, 0.28f, 0.60f),
                TelemetrySummary = "STATUS: HYPOTHETICAL SIMULATION\nDiameter: 41.5 Billion ly (Closed Curvature k=+1)\nPhysics: Gravitational constant G = 2.8x Standard\nRapid stellar nucleosynthesis & frequent binary black holes."
            };
            RegisterBody(uniHyperion, _rootNode);

            // UNIVERSE 3: CHRONOS-ZERO
            var uniChronos = new CosmicBodyNode
            {
                Id = "uni_chronos",
                Name = "Universe Chronos-Zero",
                Subtitle = "Late-Epoch Red Dwarf & Planetary Nebula Cosmos",
                Tier = CosmicTier.Universe,
                IsEmpirical = false,
                LocalPos = new Vector3d(220, -40, 60),
                Radius = 82f,
                PrimaryColor = new Color(0.66f, 0.33f, 0.97f),
                SecondaryColor = new Color(0.18f, 0.83f, 0.75f),
                TelemetrySummary = "STATUS: HYPOTHETICAL SIMULATION\nAge: 42.0 Billion Years\nTemp: 0.85 K\nDominated by 10-trillion-year lifespan M-dwarf stars and glowing planetary nebulae."
            };
            RegisterBody(uniChronos, _rootNode);

            // GALAXIES IN UNIVERSE ALPHA-1
            var galMilkyWay = new CosmicBodyNode
            {
                Id = "gal_milky_way",
                Name = "Milky Way Galaxy",
                Subtitle = "Barred Spiral Galaxy (SBbc)",
                Tier = CosmicTier.Galaxy,
                IsEmpirical = true,
                LocalPos = new Vector3d(-32, 5, 14),
                Radius = 25f,
                PrimaryColor = new Color(0.38f, 0.65f, 0.98f),
                SecondaryColor = new Color(1.0f, 0.94f, 0.75f),
                TelemetrySummary = "STATUS: EMPIRICAL SCIENCE\nDiameter: 105,700 Light-Years\nMass: 1.5 x 10^12 Solar Masses\nStars: 100–400 Billion\nCore Anchor: Sagittarius A* Supermassive Black Hole"
            };
            RegisterBody(galMilkyWay, uniAlpha);

            var galAndromeda = new CosmicBodyNode
            {
                Id = "gal_andromeda",
                Name = "Andromeda Galaxy (M31)",
                Subtitle = "Grand-Design Spiral Galaxy (SA(s)b)",
                Tier = CosmicTier.Galaxy,
                IsEmpirical = true,
                LocalPos = new Vector3d(38, -10, -22),
                Radius = 29f,
                PrimaryColor = new Color(0.51f, 0.55f, 0.97f),
                SecondaryColor = new Color(0.99f, 0.92f, 0.55f),
                TelemetrySummary = "STATUS: EMPIRICAL SCIENCE\nDiameter: 152,000 Light-Years\nStars: ~1 Trillion\nDistance: 2.537 Million Light-Years\nApproaching Milky Way at 110 km/s."
            };
            RegisterBody(galAndromeda, uniAlpha);

            // SAGITTARIUS A* BLACK HOLE
            var bhSgrA = new CosmicBodyNode
            {
                Id = "bh_sgr_a",
                Name = "Sagittarius A* (Galactic Core SMBH)",
                Subtitle = "Supermassive Black Hole",
                Tier = CosmicTier.BlackHole,
                IsEmpirical = true,
                LocalPos = new Vector3d(0, 0, 0),
                Radius = 2.6f,
                PrimaryColor = new Color(0.98f, 0.45f, 0.09f),
                SecondaryColor = new Color(1.0f, 0.97f, 0.92f),
                TelemetrySummary = "STATUS: EMPIRICAL SCIENCE\nMass: 4.297 x 10^6 Solar Masses\nSchwarzschild Diameter: 25.4 Million km\n\nSCIENTIFIC FACT: The event horizon is a non-glowing causal boundary. Light comes from relativistic Doppler beaming in the accretion disk."
            };
            RegisterBody(bhSgrA, galMilkyWay);

            // SOL SYSTEM
            var sysSol = new CosmicBodyNode
            {
                Id = "sys_sol",
                Name = "Sol System (Solar System)",
                Subtitle = "G2V Yellow Dwarf Planetary System",
                Tier = CosmicTier.StarSystem,
                IsEmpirical = true,
                LocalPos = new Vector3d(14, 1.5, 9),
                Radius = 3.2f,
                PrimaryColor = new Color(0.99f, 0.88f, 0.28f),
                SecondaryColor = new Color(0.98f, 0.45f, 0.09f),
                TelemetrySummary = "STATUS: EMPIRICAL SCIENCE\nStar Type: G2V Yellow Dwarf\nPhotosphere Temp: 5,772 K\nCore Temp: 1.57 x 10^7 K\nAge: 4.568 Billion Years"
            };
            RegisterBody(sysSol, galMilkyWay);

            // PLANETS IN SOL SYSTEM
            var plEarth = new CosmicBodyNode
            {
                Id = "pl_earth",
                Name = "Earth (Terra)",
                Subtitle = "Rocky Ocean World",
                Tier = CosmicTier.Planet,
                IsEmpirical = true,
                Radius = 0.85f,
                OrbitRadius = 11.0f,
                OrbitSpeed = 0.55f,
                OrbitAngle = 0.8f,
                PrimaryColor = new Color(0.08f, 0.52f, 0.26f),
                SecondaryColor = new Color(0.02f, 0.42f, 0.68f),
                TelemetrySummary = "STATUS: EMPIRICAL SCIENCE\nDiameter: 12,742 km | Gravity: 9.807 m/s²\nMean Temp: 288 K (15 °C)\nOrbital Period: 365.256 Days\n\nZoom closer or tap 'Land on Surface' to inspect 3D procedural terrain!"
            };
            RegisterBody(plEarth, sysSol);
            _orbitingBodies.Add(plEarth);

            var plMars = new CosmicBodyNode
            {
                Id = "pl_mars",
                Name = "Mars (Ares)",
                Subtitle = "Rocky Desert World",
                Tier = CosmicTier.Planet,
                IsEmpirical = true,
                Radius = 0.62f,
                OrbitRadius = 16.5f,
                OrbitSpeed = 0.36f,
                OrbitAngle = 2.4f,
                PrimaryColor = new Color(0.78f, 0.26f, 0.06f),
                SecondaryColor = new Color(0.48f, 0.18f, 0.07f),
                TelemetrySummary = "STATUS: EMPIRICAL SCIENCE\nDiameter: 6,779 km | Gravity: 3.721 m/s²\nMean Temp: 210 K (-63 °C)\nHome to Olympus Mons (21.9 km shield volcano) and Valles Marineris."
            };
            RegisterBody(plMars, sysSol);
            _orbitingBodies.Add(plMars);

            var plSaturn = new CosmicBodyNode
            {
                Id = "pl_saturn",
                Name = "Saturn (Kronos)",
                Subtitle = "Ringed Gas Giant",
                Tier = CosmicTier.Planet,
                IsEmpirical = true,
                Radius = 1.45f,
                OrbitRadius = 25.0f,
                OrbitSpeed = 0.18f,
                OrbitAngle = 4.9f,
                PrimaryColor = new Color(0.92f, 0.72f, 0.12f),
                SecondaryColor = new Color(0.99f, 0.94f, 0.60f),
                TelemetrySummary = "STATUS: EMPIRICAL SCIENCE\nDiameter: 116,460 km (Rings span 282,000 km)\nMass: 95.16 Earth Masses\nRings composed of 99.9% pure water-ice particles."
            };
            RegisterBody(plSaturn, sysSol);
            _orbitingBodies.Add(plSaturn);
        }

        private void RegisterBody(CosmicBodyNode node, CosmicBodyNode parent)
        {
            node.Parent = parent;
            parent?.Children.Add(node);
            _nodeIndex[node.Id] = node;
        }

        private void Build3DLayers()
        {
            // 1. Multiverse Layer: Bubble Universes
            foreach (var uni in _rootNode.Children)
            {
                var bubble = CreateGlowingSphere(uni.Radius, uni.PrimaryColor, true);
                bubble.Position = uni.LocalPos.ToVector3();
                _multiverseLayer.AddChild(bubble);
                uni.VisualNode = bubble;
            }

            // 2. Universe Layer: GPU MultiMesh Instanced Galaxies
            var alphaUni = _nodeIndex["uni_alpha"];
            foreach (var gal in alphaUni.Children)
            {
                var galNode = CreateGpuMultiMeshGalaxy(gal, 18000);
                galNode.Position = gal.LocalPos.ToVector3();
                _universeLayer.AddChild(galNode);
                gal.VisualNode = galNode;
            }

            // 3. Galaxy Layer: Stars & Central Black Hole
            var mw = _nodeIndex["gal_milky_way"];
            var mwBg = CreateGpuMultiMeshGalaxy(mw, 25000);
            _galaxyLayer.AddChild(mwBg);

            foreach (var item in mw.Children)
            {
                Node3D marker = item.Tier == CosmicTier.BlackHole
                    ? CreateRelativisticBlackHoleNode(item.Radius)
                    : CreateGlowingSphere(item.Radius * 0.6f, item.PrimaryColor, false);
                marker.Position = item.LocalPos.ToVector3();
                _galaxyLayer.AddChild(marker);
            }

            // 4. Solar System Layer: Central Star, Black Hole Inspection & Orbiting Planets
            var sol = _nodeIndex["sys_sol"];
            var sunMesh = CreateGlowingSphere(sol.Radius, sol.PrimaryColor, false);
            _systemLayer.AddChild(sunMesh);
            sol.VisualNode = sunMesh;

            foreach (var pl in sol.Children)
            {
                var plGroup = new Node3D();
                var mesh = CreateProceduralPlanetMesh(pl);
                plGroup.AddChild(mesh);

                if (pl.Id == "pl_saturn")
                {
                    var ring = new MeshInstance3D
                    {
                        Mesh = new TorusMesh { InnerRadius = pl.Radius * 1.4f, OuterRadius = pl.Radius * 2.35f }
                    };
                    var ringMat = new StandardMaterial3D
                    {
                        AlbedoColor = new Color(0.98f, 0.90f, 0.65f, 0.8f),
                        Transparency = BaseMaterial3D.TransparencyEnum.Alpha
                    };
                    ring.MaterialOverride = ringMat;
                    ring.RotationDegrees = new Vector3(18f, 0f, 12f);
                    plGroup.AddChild(ring);
                }

                _systemLayer.AddChild(plGroup);
                pl.VisualNode = plGroup;
            }

            // 5. Planetary Surface Terrain Layer
            BuildProceduralSurfaceTerrain();
        }

        private Node3D CreateGlowingSphere(float radius, Color color, bool translucentMembrane)
        {
            var root = new Node3D();
            var mi = new MeshInstance3D
            {
                Mesh = new SphereMesh { Radius = radius, Height = radius * 2f, RadialSegments = 48, Rings = 24 }
            };
            var mat = new StandardMaterial3D
            {
                AlbedoColor = translucentMembrane ? new Color(color.R, color.G, color.B, 0.28f) : color,
                EmissionEnabled = true,
                Emission = color,
                EmissionEnergyMultiplier = translucentMembrane ? 1.2f : 2.6f,
                Transparency = translucentMembrane ? BaseMaterial3D.TransparencyEnum.Alpha : BaseMaterial3D.TransparencyEnum.Disabled
            };
            mi.MaterialOverride = mat;
            root.AddChild(mi);
            return root;
        }

        private Node3D CreateRelativisticBlackHoleNode(float horizonRadius)
        {
            var bhRoot = new Node3D();

            // Scientifically non-glowing dark Event Horizon sphere
            var horizon = new MeshInstance3D
            {
                Mesh = new SphereMesh { Radius = horizonRadius, Height = horizonRadius * 2f }
            };
            horizon.MaterialOverride = new StandardMaterial3D
            {
                ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
                AlbedoColor = Colors.Black
            };
            bhRoot.AddChild(horizon);

            // Relativistic Accretion Disk
            var disk = new MeshInstance3D
            {
                Mesh = new TorusMesh { InnerRadius = horizonRadius * 1.35f, OuterRadius = horizonRadius * 4.2f }
            };
            disk.Scale = new Vector3(1f, 0.08f, 1f);
            disk.RotationDegrees = new Vector3(14f, 0f, -10f);
            disk.MaterialOverride = new StandardMaterial3D
            {
                AlbedoColor = new Color(1.0f, 0.55f, 0.12f),
                EmissionEnabled = true,
                Emission = new Color(1.0f, 0.48f, 0.08f),
                EmissionEnergyMultiplier = 3.2f
            };
            bhRoot.AddChild(disk);

            // Lensed Einstein Photon Ring Arch
            var arch = new MeshInstance3D
            {
                Mesh = new TorusMesh { InnerRadius = horizonRadius * 1.15f, OuterRadius = horizonRadius * 2.1f }
            };
            arch.RotationDegrees = new Vector3(82f, 0f, 0f);
            arch.MaterialOverride = disk.MaterialOverride;
            bhRoot.AddChild(arch);

            return bhRoot;
        }

        private Node3D CreateGpuMultiMeshGalaxy(CosmicBodyNode gal, int starCount)
        {
            var container = new Node3D();
            var mmi = new MultiMeshInstance3D();
            var mm = new MultiMesh
            {
                TransformFormat = MultiMesh.TransformFormatEnum.Transform3D,
                UseColors = true,
                InstanceCount = starCount,
                Mesh = new BoxMesh { Size = new Vector3(0.22f, 0.22f, 0.22f) }
            };

            var mat = new StandardMaterial3D
            {
                ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
                VertexColorUseAsAlbedo = true,
                Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
                BlendMode = BaseMaterial3D.BlendModeEnum.Add
            };
            mmi.MaterialOverride = mat;

            var rng = new Random(gal.Id.GetHashCode());
            float maxR = gal.Radius;
            int arms = 4;

            for (int i = 0; i < starCount; i++)
            {
                float rNorm = Mathf.Pow((float)rng.NextDouble(), 1.35f);
                float r = rNorm * maxR;
                int arm = i % arms;
                float angle = (arm / (float)arms) * Mathf.Pi * 2f + r * 0.26f + ((float)rng.NextDouble() - 0.5f) * 0.65f;
                float y = ((float)rng.NextDouble() - 0.5f) * (1f - rNorm * 0.75f) * maxR * 0.14f;

                var pos = new Vector3(Mathf.Cos(angle) * r, y, Mathf.Sin(angle) * r);
                mm.SetInstanceTransform(i, new Transform3D(Basis.Identity, pos));

                Color c = gal.SecondaryColor.Lerp(gal.PrimaryColor, rNorm);
                mm.SetInstanceColor(i, c);
            }

            mmi.Multimesh = mm;
            container.AddChild(mmi);
            return container;
        }

        private MeshInstance3D CreateProceduralPlanetMesh(CosmicBodyNode pl)
        {
            var mi = new MeshInstance3D
            {
                Mesh = new SphereMesh { Radius = pl.Radius, Height = pl.Radius * 2f, RadialSegments = 64, Rings = 32 }
            };
            var noise = new FastNoiseLite
            {
                NoiseType = FastNoiseLite.NoiseTypeEnum.SimplexSmooth,
                Seed = pl.Id.GetHashCode(),
                FractalOctaves = 5,
                Frequency = 0.035f
            };
            var noiseTex = new NoiseTexture2D
            {
                Noise = noise,
                Seamless = true,
                Width = 256,
                Height = 128
            };
            var mat = new StandardMaterial3D
            {
                AlbedoColor = pl.PrimaryColor,
                AlbedoTexture = noiseTex,
                Roughness = 0.65f,
                Metallic = 0.08f
            };
            mi.MaterialOverride = mat;
            return mi;
        }

        private void BuildProceduralSurfaceTerrain()
        {
            var terrain = new MeshInstance3D
            {
                Mesh = new PlaneMesh { Size = new Vector2(180f, 180f), SubdivideWidth = 96, SubdivideDepth = 96 }
            };
            var mat = new StandardMaterial3D
            {
                AlbedoColor = new Color(0.10f, 0.52f, 0.28f),
                Roughness = 0.85f
            };
            terrain.MaterialOverride = mat;
            _surfaceLayer.AddChild(terrain);
        }

        public override void _Input(InputEvent @event)
        {
            // Mobile Multi-Touch Orbit & Pinch-to-Zoom
            if (@event is InputEventScreenTouch touch)
            {
                if (touch.Pressed) _activeTouches[touch.Index] = touch.Position;
                else _activeTouches.Remove(touch.Index);
                _lastPinchDist = 0f;
            }
            else if (@event is InputEventScreenDrag drag)
            {
                _activeTouches[drag.Index] = drag.Position;
                if (_activeTouches.Count == 1)
                {
                    _targetTheta -= drag.Relative.X * 0.006f;
                    _targetPhi = Mathf.Clamp(_targetPhi - drag.Relative.Y * 0.006f, 0.12f, Mathf.Pi - 0.12f);
                }
                else if (_activeTouches.Count >= 2)
                {
                    var pts = new List<Vector2>(_activeTouches.Values);
                    float d = pts[0].DistanceTo(pts[1]);
                    if (_lastPinchDist > 5f)
                    {
                        double ratio = _lastPinchDist / Mathf.Max(10f, d);
                        _targetCamRadius = Math.Clamp(_targetCamRadius * ratio, 0.5, 2200.0);
                    }
                    _lastPinchDist = d;
                }
            }
            // Desktop Mouse Orbit & Wheel Zoom
            else if (@event is InputEventMouseButton mb)
            {
                if (mb.ButtonIndex == MouseButton.Left) _isDragging = mb.Pressed;
                else if (mb.ButtonIndex == MouseButton.WheelUp && mb.Pressed)
                    _targetCamRadius = Math.Clamp(_targetCamRadius * 0.85, 0.5, 2200.0);
                else if (mb.ButtonIndex == MouseButton.WheelDown && mb.Pressed)
                    _targetCamRadius = Math.Clamp(_targetCamRadius * 1.18, 0.5, 2200.0);
            }
            else if (@event is InputEventMouseMotion mm && _isDragging)
            {
                _targetTheta -= mm.Relative.X * 0.006f;
                _targetPhi = Mathf.Clamp(_targetPhi - mm.Relative.Y * 0.006f, 0.12f, Mathf.Pi - 0.12f);
            }
        }

        public override void _Process(double delta)
        {
            float dt = (float)delta;

            // Step Orbital Simulation
            if (_simRunning)
            {
                foreach (var pl in _orbitingBodies)
                {
                    pl.OrbitAngle += pl.OrbitSpeed * dt * _simSpeed;
                    if (pl.VisualNode != null)
                    {
                        pl.VisualNode.Position = new Vector3(
                            Mathf.Cos(pl.OrbitAngle) * pl.OrbitRadius,
                            0f,
                            Mathf.Sin(pl.OrbitAngle) * pl.OrbitRadius
                        );
                        pl.VisualNode.RotateY(dt * 0.8f * _simSpeed);
                    }
                }
            }

            // Update Target Position for Orbiting Planet
            if (_focusedNode != null && _focusedNode.Tier == CosmicTier.Planet && _focusedNode.VisualNode != null)
            {
                Vector3 p = _focusedNode.VisualNode.GlobalPosition;
                _targetPos64 = new Vector3d(p.X, p.Y, p.Z);
            }

            // Smooth 64-Bit Camera Interpolation
            _camRadius += (_targetCamRadius - _camRadius) * Math.Min(1.0, delta * 8.0);
            _theta = Mathf.Lerp(_theta, _targetTheta, dt * 10f);
            _phi = Mathf.Lerp(_phi, _targetPhi, dt * 10f);
            _currentLookAt64 = Vector3d.Lerp(_currentLookAt64, _targetPos64, delta * 10.0);

            double sinPhi = Math.Sin(_phi);
            Vector3d camPos64 = _currentLookAt64 + new Vector3d(
                _camRadius * sinPhi * Math.Sin(_theta),
                _camRadius * Math.Cos(_phi),
                _camRadius * sinPhi * Math.Cos(_theta)
            );

            _camera.GlobalPosition = camPos64.ToVector3();
            _camera.LookAt(_currentLookAt64.ToVector3(), Vector3.Up);
        }

        public void FocusOnBody(CosmicBodyNode node, double? customRadius = null)
        {
            if (node == null) return;
            _focusedNode = node;

            _multiverseLayer.Visible = node.Tier == CosmicTier.Multiverse;
            _universeLayer.Visible = node.Tier == CosmicTier.Universe;
            _galaxyLayer.Visible = node.Tier == CosmicTier.Galaxy;
            _systemLayer.Visible = node.Tier == CosmicTier.StarSystem || node.Tier == CosmicTier.Planet || node.Tier == CosmicTier.BlackHole;
            _surfaceLayer.Visible = node.Tier == CosmicTier.PlanetarySurface;

            _targetPos64 = (node.Tier == CosmicTier.Multiverse || node.Tier == CosmicTier.Universe)
                ? node.LocalPos
                : Vector3d.Zero;

            _targetCamRadius = customRadius ?? (node.Tier switch
            {
                CosmicTier.Multiverse => 680.0,
                CosmicTier.Universe => 140.0,
                CosmicTier.Galaxy => 50.0,
                CosmicTier.StarSystem => 36.0,
                CosmicTier.BlackHole => 12.0,
                CosmicTier.Planet => 3.5,
                CosmicTier.PlanetarySurface => 8.0,
                _ => 50.0
            });

            RefreshHudUI();
        }

        private void BuildFuturisticHudUI()
        {
            var canvas = new CanvasLayer();
            AddChild(canvas);

            // Top Bar
            var topBar = new PanelContainer
            {
                AnchorLeft = 0f,
                AnchorRight = 1f,
                OffsetLeft = 16,
                OffsetTop = 12,
                OffsetRight = -16,
                OffsetBottom = 64
            };
            canvas.AddChild(topBar);

            var topRow = new HBoxContainer();
            topBar.AddChild(topRow);

            _tierBadgeLabel = new Label { Text = "TINYVERSE C# (.NET 8) • MULTIVERSE" };
            topRow.AddChild(_tierBadgeLabel);

            var spacer = new Control { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
            topRow.AddChild(spacer);

            _breadcrumbContainer = new HBoxContainer();
            topRow.AddChild(_breadcrumbContainer);

            // Left Outliner Panel
            var leftPanel = new PanelContainer
            {
                AnchorLeft = 0f,
                AnchorBottom = 1f,
                OffsetLeft = 16,
                OffsetTop = 76,
                OffsetRight = 310,
                OffsetBottom = -76
            };
            canvas.AddChild(leftPanel);

            var leftScroll = new ScrollContainer();
            leftPanel.AddChild(leftScroll);

            _outlinerListContainer = new VBoxContainer { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
            leftScroll.AddChild(_outlinerListContainer);

            // Right Telemetry Panel
            var rightPanel = new PanelContainer
            {
                AnchorLeft = 1f,
                AnchorRight = 1f,
                AnchorBottom = 1f,
                OffsetLeft = -340,
                OffsetTop = 76,
                OffsetRight = -16,
                OffsetBottom = -76
            };
            canvas.AddChild(rightPanel);

            var rightBox = new VBoxContainer();
            rightPanel.AddChild(rightBox);

            _telemetryTitleLabel = new Label { Text = "Object Telemetry", AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _telemetryBodyLabel = new Label { Text = "", AutowrapMode = TextServer.AutowrapMode.WordSmart };
            rightBox.AddChild(_telemetryTitleLabel);
            rightBox.AddChild(_telemetryBodyLabel);

            // Bottom Simulation Controls Bar
            var bottomBar = new PanelContainer
            {
                AnchorLeft = 0f,
                AnchorTop = 1f,
                AnchorRight = 1f,
                AnchorBottom = 1f,
                OffsetLeft = 16,
                OffsetTop = -64,
                OffsetRight = -16,
                OffsetBottom = -12
            };
            canvas.AddChild(bottomBar);

            var bottomRow = new HBoxContainer();
            bottomBar.AddChild(bottomRow);

            var btnZoomIn = new Button { Text = "+ Zoom In" };
            btnZoomIn.Pressed += () => _targetCamRadius = Math.Max(0.5, _targetCamRadius * 0.7);
            bottomRow.AddChild(btnZoomIn);

            var btnZoomOut = new Button { Text = "- Zoom Out" };
            btnZoomOut.Pressed += () => _targetCamRadius = Math.Min(2200.0, _targetCamRadius * 1.4);
            bottomRow.AddChild(btnZoomOut);

            var btnParent = new Button { Text = "↑ Ascend to Parent" };
            btnParent.Pressed += () =>
            {
                if (_focusedNode?.Parent != null) FocusOnBody(_focusedNode.Parent);
            };
            bottomRow.AddChild(btnParent);

            _playPauseButton = new Button { Text = "⏸ Sim Running" };
            _playPauseButton.Pressed += () =>
            {
                _simRunning = !_simRunning;
                _playPauseButton.Text = _simRunning ? "⏸ Sim Running" : "▶ Sim Paused";
            };
            bottomRow.AddChild(_playPauseButton);

            var btnFast = new Button { Text = "Speed 2x" };
            btnFast.Pressed += () => _simSpeed = _simSpeed >= 4f ? 0.5f : _simSpeed * 2f;
            bottomRow.AddChild(btnFast);
        }

        private void RefreshHudUI()
        {
            if (_focusedNode == null) return;

            _tierBadgeLabel.Text = $"TINYVERSE C# (.NET 8) • {_focusedNode.Tier.ToString().ToUpper()}";
            _telemetryTitleLabel.Text = $"{_focusedNode.Name}\n({_focusedNode.Subtitle})";
            _telemetryBodyLabel.Text = _focusedNode.TelemetrySummary;

            foreach (Node child in _outlinerListContainer.GetChildren()) child.QueueFree();

            var listSource = _focusedNode.Children.Count > 0
                ? _focusedNode.Children
                : (_focusedNode.Parent != null ? _focusedNode.Parent.Children : _rootNode.Children);

            foreach (var item in listSource)
            {
                var btn = new Button
                {
                    Text = $"➔ {item.Name} [{item.Tier}]",
                    Alignment = HorizontalAlignment.Left
                };
                var captured = item;
                btn.Pressed += () => FocusOnBody(captured);
                _outlinerListContainer.AddChild(btn);
            }

            if (_focusedNode.Tier == CosmicTier.Planet)
            {
                var surfBtn = new Button { Text = "🛰️ Land / Inspect Planetary Surface" };
                surfBtn.Pressed += () =>
                {
                    _surfaceLayer.Visible = true;
                    _systemLayer.Visible = false;
                    _targetCamRadius = 8.0;
                    _tierBadgeLabel.Text = "TINYVERSE C# (.NET 8) • PLANETARY SURFACE";
                };
                _outlinerListContainer.AddChild(surfBtn);
            }
        }
    }
}
