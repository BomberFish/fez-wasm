using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common;
using FezEngine.Components;
using FezEngine.Tools;
using FezGame;
using FezEngine;
using FezGame.Services;
using FezGame.Services.Scripting;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

[assembly: System.Runtime.Versioning.SupportedOSPlatform("browser")]

partial class Program
{
	private static void Main()
	{
		Console.WriteLine("Hello from WASM world!");
		Logger.Log("WASM", "Logger.Log");

		// find ./Fez-Src/FezEngine/FezEngine.Readers -type f -name "*.cs" | awk -F'/' '{filename=$NF; gsub(".cs$", "", filename); print "ContentTypeReaderManager.AddCustomType(\"FezEngine.Readers." filename ", FezEngine\", typeof(FezEngine.Readers." filename "));"}' | sort
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.AmbienceTrackReader, FezEngine", typeof(FezEngine.Readers.AmbienceTrackReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.AnimatedTextureReader, FezEngine", typeof(FezEngine.Readers.AnimatedTextureReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.ArtObjectActorSettingsReader, FezEngine", typeof(FezEngine.Readers.ArtObjectActorSettingsReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.ArtObjectInstanceReader, FezEngine", typeof(FezEngine.Readers.ArtObjectInstanceReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.ArtObjectReader, FezEngine", typeof(FezEngine.Readers.ArtObjectReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.BackgroundPlaneReader, FezEngine", typeof(FezEngine.Readers.BackgroundPlaneReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.CameraNodeDataReader, FezEngine", typeof(FezEngine.Readers.CameraNodeDataReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.DotDialogueLineReader, FezEngine", typeof(FezEngine.Readers.DotDialogueLineReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.EntityReader, FezEngine", typeof(FezEngine.Readers.EntityReader));
		// ContentTypeReaderManager.AddCustomType("FezEngine.Readers.ExtensibleReader, FezEngine", typeof(FezEngine.Readers.ExtensibleReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.FezVertexPositionNormalTextureReader, FezEngine", typeof(FezEngine.Readers.FezVertexPositionNormalTextureReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.FrameReader, FezEngine", typeof(FezEngine.Readers.FrameReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.FutureTexture2DReader, FezEngine", typeof(FezEngine.Readers.FutureTexture2DReader));
		// ContentTypeReaderManager.AddCustomType("FezEngine.Readers.IndexedUserPrimitivesReader, FezEngine", typeof(FezEngine.Readers.IndexedUserPrimitivesReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.InstanceActorSettingsReader, FezEngine", typeof(FezEngine.Readers.InstanceActorSettingsReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.LevelReader, FezEngine", typeof(FezEngine.Readers.LevelReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.LoopReader, FezEngine", typeof(FezEngine.Readers.LoopReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.MapNodeConnectionReader, FezEngine", typeof(FezEngine.Readers.MapNodeConnectionReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.MapNodeReader, FezEngine", typeof(FezEngine.Readers.MapNodeReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.MapTreeReader, FezEngine", typeof(FezEngine.Readers.MapTreeReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.MovementPathReader, FezEngine", typeof(FezEngine.Readers.MovementPathReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.NpcActionContentReader, FezEngine", typeof(FezEngine.Readers.NpcActionContentReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.NpcInstanceReader, FezEngine", typeof(FezEngine.Readers.NpcInstanceReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.NpcMetadataReader, FezEngine", typeof(FezEngine.Readers.NpcMetadataReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.PathSegmentReader, FezEngine", typeof(FezEngine.Readers.PathSegmentReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.RectangularTrixelSurfacePartReader, FezEngine", typeof(FezEngine.Readers.RectangularTrixelSurfacePartReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.ScriptActionReader, FezEngine", typeof(FezEngine.Readers.ScriptActionReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.ScriptConditionReader, FezEngine", typeof(FezEngine.Readers.ScriptConditionReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.ScriptReader, FezEngine", typeof(FezEngine.Readers.ScriptReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.ScriptTriggerReader, FezEngine", typeof(FezEngine.Readers.ScriptTriggerReader));
		// ContentTypeReaderManager.AddCustomType("FezEngine.Readers.ShaderInstancedIndexedPrimitivesReader, FezEngine", typeof(FezEngine.Readers.ShaderInstancedIndexedPrimitivesReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.SkyLayerReader, FezEngine", typeof(FezEngine.Readers.SkyLayerReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.SkyReader, FezEngine", typeof(FezEngine.Readers.SkyReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.SpeechLineReader, FezEngine", typeof(FezEngine.Readers.SpeechLineReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.TrackedSongReader, FezEngine", typeof(FezEngine.Readers.TrackedSongReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.TrileEmplacementReader, FezEngine", typeof(FezEngine.Readers.TrileEmplacementReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.TrileFaceReader, FezEngine", typeof(FezEngine.Readers.TrileFaceReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.TrileGroupReader, FezEngine", typeof(FezEngine.Readers.TrileGroupReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.TrileInstanceReader, FezEngine", typeof(FezEngine.Readers.TrileInstanceReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.TrileReader, FezEngine", typeof(FezEngine.Readers.TrileReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.TrileSetReader, FezEngine", typeof(FezEngine.Readers.TrileSetReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.TrixelIdentifierReader, FezEngine", typeof(FezEngine.Readers.TrixelIdentifierReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.VertexPositionNormalTextureInstanceReader, FezEngine", typeof(FezEngine.Readers.VertexPositionNormalTextureInstanceReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.VolumeActorSettingsReader, FezEngine", typeof(FezEngine.Readers.VolumeActorSettingsReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.VolumeReader, FezEngine", typeof(FezEngine.Readers.VolumeReader));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.WinConditionsReader, FezEngine", typeof(FezEngine.Readers.WinConditionsReader));

		// manually added
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Loop, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Loop>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ArrayReader`1[[FezEngine.Structure.ShardNotes, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ArrayReader<FezEngine.Structure.ShardNotes>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.ShardNotes, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.ShardNotes>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.AssembleChords, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.AssembleChords>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.FaceOrientation, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.FaceOrientation>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.LiquidType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.LiquidType>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.Volume, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.Volume>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ArrayReader`1[[FezEngine.FaceOrientation, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ArrayReader<FezEngine.FaceOrientation>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.Scripting.Script, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.Scripting.Script>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Scripting.ScriptTrigger, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Scripting.ScriptTrigger>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Scripting.ScriptAction, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Scripting.ScriptAction>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.DictionaryReader`2[[FezEngine.Structure.TrileEmplacement, FezEngine],[FezEngine.Structure.TrileInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<FezEngine.Structure.TrileEmplacement, FezEngine.Structure.TrileInstance>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.ArtObjectInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.ArtObjectInstance>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.ActorType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.ActorType>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Viewpoint, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Viewpoint>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.BackgroundPlane, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.BackgroundPlane>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.TrileGroup, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.TrileGroup>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.NpcInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.NpcInstance>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.MovementPath, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.MovementPath>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.AmbienceTrack, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.AmbienceTrack>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.LevelNodeType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.LevelNodeType>));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.ShaderInstancedIndexedPrimitivesReader`2[[FezEngine.Structure.Geometry.VertexPositionNormalTextureInstance, FezEngine],[Microsoft.Xna.Framework.Matrix, FNA, Version=24.1.0.0, Culture=neutral, PublicKeyToken=null]], FezEngine", typeof(FezEngine.Readers.ShaderInstancedIndexedPrimitivesReader<FezEngine.Structure.Geometry.VertexPositionNormalTextureInstance, Microsoft.Xna.Framework.Matrix>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ArrayReader`1[[FezEngine.Structure.Geometry.VertexPositionNormalTextureInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ArrayReader<FezEngine.Structure.Geometry.VertexPositionNormalTextureInstance>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.SkyLayer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.SkyLayer>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.Trile, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.Trile>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.DictionaryReader`2[[FezEngine.FaceOrientation, FezEngine],[FezEngine.CollisionType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<FezEngine.FaceOrientation, FezEngine.CollisionType>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.CollisionType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.CollisionType>));
		ContentTypeReaderManager.AddCustomType("FezEngine.Readers.ShaderInstancedIndexedPrimitivesReader`2[[FezEngine.Structure.Geometry.VertexPositionNormalTextureInstance, FezEngine],[Microsoft.Xna.Framework.Vector4, FNA, Version=24.1.0.0, Culture=neutral, PublicKeyToken=null]], FezEngine", typeof(FezEngine.Readers.ShaderInstancedIndexedPrimitivesReader<FezEngine.Structure.Geometry.VertexPositionNormalTextureInstance, Microsoft.Xna.Framework.Vector4>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.SurfaceType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.SurfaceType>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Scripting.ScriptCondition, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Scripting.ScriptCondition>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.Scripting.ComparisonOperator, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.Scripting.ComparisonOperator>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Scripting.Script, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Scripting.Script>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrileInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrileInstance>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.NpcInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.NpcInstance>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.NpcActionContent, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.NpcActionContent>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.NpcMetadata, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.NpcMetadata>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.MovementPath, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.MovementPath>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.MapNode, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.MapNode>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.PathSegment, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.PathSegment>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.MapTree, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.MapTree>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Level, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Level>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.InstanceActorSettings, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.InstanceActorSettings>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.PathEndBehavior, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.PathEndBehavior>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.DictionaryReader`2[[FezEngine.Structure.NpcAction, FezEngine],[FezEngine.Structure.NpcActionContent, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<FezEngine.Structure.NpcAction, FezEngine.Structure.NpcActionContent>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.NpcAction, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.NpcAction>));


		// ls Fez-Src/FezEngine/FezEngine.Components/*.cs | sed 's|.*/||; s|\.cs$||' | while read name; do echo "ContentTypeReaderManager.AddCustomType(\"Microsoft.Xna.Framework.Content.ListReader\`1[[FezEngine.Components.$name, FezEngine]]\", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.$name>));"; done
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.ActiveAmbience, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.ActiveAmbience>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.ActiveAmbienceTrack, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.ActiveAmbienceTrack>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.ActiveLoop, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.ActiveLoop>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.ActiveTrackedSong, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.ActiveTrackedSong>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.AnimatedPlanesHost, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.AnimatedPlanesHost>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.BurnInPostProcess, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.BurnInPostProcess>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.CamShake, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.CamShake>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.CameraPathsHost, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.CameraPathsHost>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.CloudLayerExtensions, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.CloudLayerExtensions>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.CloudShadowsHost, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.CloudShadowsHost>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.FarawayPlaceData, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.FarawayPlaceData>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.FarawayPlaceHost, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.FarawayPlaceHost>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.FontManager, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.FontManager>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.FpsMeasurer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.FpsMeasurer>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.GammaCorrection, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.GammaCorrection>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.GlyphTextRenderer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.GlyphTextRenderer>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.IFontManager, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.IFontManager>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.IInputManager, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.IInputManager>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.ILightingPostProcess, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.ILightingPostProcess>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.IWaiter, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.IWaiter>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.InputManager, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.InputManager>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.Layer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.Layer>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.LayerComparer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.LayerComparer>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.LevelHost, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.LevelHost>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.LightingPostProcess, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.LightingPostProcess>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.MovingGroupsHost, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.MovingGroupsHost>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.NpcHost, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.NpcHost>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.NpcState, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.NpcState>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.PlaneParticleSystem, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.PlaneParticleSystem>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.PlaneParticleSystemSettings, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.PlaneParticleSystemSettings>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.RenderWaiter, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.RenderWaiter>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.ScreenFade, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.ScreenFade>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.ScreenshotTaker, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.ScreenshotTaker>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.Sequencer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.Sequencer>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.SkyHost, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.SkyHost>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.TimeHost, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.TimeHost>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.UnsafeAreaOverlayComponent, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.UnsafeAreaOverlayComponent>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.UpdateWaiter, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.UpdateWaiter>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.VaryingColor, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.VaryingColor>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.VaryingSingle, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.VaryingSingle>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.VaryingValue, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.VaryingValue>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.VaryingVector3, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.VaryingVector3>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.Waiter, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.Waiter>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Components.Waiters, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Components.Waiters>));


		// ls Fez-Src/FezEngine/FezEngine.Structure/*.cs | sed 's|.*/||; s|\.cs$||' | while read name; do echo "ContentTypeReaderManager.AddCustomType(\"Microsoft.Xna.Framework.Content.ListReader\`1[[FezEngine.Structure.$name, FezEngine]]\", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.$name>));"; done
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.ActorType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.ActorType>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.ActorTypeExtensions, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.ActorTypeExtensions>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.AmbienceTrack, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.AmbienceTrack>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.AnimatedTexture, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.AnimatedTexture>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.ArtObject, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.ArtObject>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.ArtObjectActorSettings, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.ArtObjectActorSettings>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.ArtObjectCustomData, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.ArtObjectCustomData>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.ArtObjectInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.ArtObjectInstance>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.AssembleChords, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.AssembleChords>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.BackgroundPlane, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.BackgroundPlane>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.BillboardingMode, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.BillboardingMode>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.CameraNodeData, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.CameraNodeData>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.CollisionResult, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.CollisionResult>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.DayPhase, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.DayPhase>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.DayPhaseExtensions, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.DayPhaseExtensions>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.DirectionalLight, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.DirectionalLight>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Dirtyable, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Dirtyable>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.DotDialogueLine, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.DotDialogueLine>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.FaceMaterialization, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.FaceMaterialization>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.FutureTexture2D, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.FutureTexture2D>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Group, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Group>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.IPhysicsEntity, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.IPhysicsEntity>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.ISimplePhysicsEntity, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.ISimplePhysicsEntity>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.ISpatialStructure, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.ISpatialStructure>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.ITrixelObject, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.ITrixelObject>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.IdentifierPool, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.IdentifierPool>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.InstanceActorSettings, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.InstanceActorSettings>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.InstanceFace, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.InstanceFace>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.InstancePhysicsState, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.InstancePhysicsState>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Level, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Level>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.LiquidType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.LiquidType>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.LiquidTypeComparer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.LiquidTypeComparer>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.LiquidTypeExtensions, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.LiquidTypeExtensions>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Loop, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Loop>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.MapNode, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.MapNode>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.MapTree, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.MapTree>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Material, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Material>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Mesh, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Mesh>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.MovementPath, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.MovementPath>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.MultipleHits, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.MultipleHits>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.NpcAction, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.NpcAction>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.NpcActionComparer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.NpcActionComparer>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.NpcActionContent, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.NpcActionContent>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.NpcActionExtensions, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.NpcActionExtensions>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.NpcInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.NpcInstance>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.NpcMetadata, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.NpcMetadata>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.OggStream, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.OggStream>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.PathEndBehavior, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.PathEndBehavior>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.PathSegment, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.PathSegment>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.PlaneCustomData, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.PlaneCustomData>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.PlayerIndexComparer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.PlayerIndexComparer>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.PointCollision, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.PointCollision>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.PointLight, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.PointLight>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.RectangularTrixelSurfacePart, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.RectangularTrixelSurfacePart>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.ShardNoteComparer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.ShardNoteComparer>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.ShardNotes, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.ShardNotes>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Sky, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Sky>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.SkyLayer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.SkyLayer>));
		// ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.SoundEffectExtensions, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.SoundEffectExtensions>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.SoundEmitter, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.SoundEmitter>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.SpeechLine, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.SpeechLine>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.StencilMask, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.StencilMask>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.SurfaceType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.SurfaceType>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.SurfaceTypeComparer, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.SurfaceTypeComparer>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TexturingType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TexturingType>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrackedSong, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrackedSong>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Trile, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Trile>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrileActorSettings, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrileActorSettings>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrileCustomData, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrileCustomData>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrileEmplacement, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrileEmplacement>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrileFace, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrileFace>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrileGroup, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrileGroup>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrileInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrileInstance>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrileInstanceData, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrileInstanceData>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrileSet, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrileSet>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrixelCluster, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrixelCluster>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrixelEmplacement, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrixelEmplacement>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrixelFace, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrixelFace>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.TrixelSurface, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.TrixelSurface>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.VirtualTrile, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.VirtualTrile>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Volume, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Volume>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.VolumeActorSettings, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.VolumeActorSettings>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.VolumeLevel, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.VolumeLevel>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.VolumeLevels, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.VolumeLevels>));
		ContentTypeReaderManager.AddCustomType("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.WinConditions, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.WinConditions>));

		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.Volume), new FezEngine.Readers.VolumeReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.TrileInstance), new FezEngine.Readers.TrileInstanceReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.TrileGroup), new FezEngine.Readers.TrileGroupReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.TrileFace), new FezEngine.Readers.TrileFaceReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.TrileEmplacement), new FezEngine.Readers.TrileEmplacementReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.Trile), new FezEngine.Readers.TrileReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.SpeechLine), new FezEngine.Readers.SpeechLineReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.Sky), new FezEngine.Readers.SkyReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.SkyLayer), new FezEngine.Readers.SkyLayerReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.RectangularTrixelSurfacePart), new FezEngine.Readers.RectangularTrixelSurfacePartReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.PathSegment), new FezEngine.Readers.PathSegmentReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.NpcMetadata), new FezEngine.Readers.NpcMetadataReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.NpcInstance), new FezEngine.Readers.NpcInstanceReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.NpcActionContent), new FezEngine.Readers.NpcActionContentReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.MovementPath), new FezEngine.Readers.MovementPathReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.MapTree), new FezEngine.Readers.MapTreeReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.MapNode), new FezEngine.Readers.MapNodeReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.Level), new FezEngine.Readers.LevelReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.InstanceActorSettings), new FezEngine.Readers.InstanceActorSettingsReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.DotDialogueLine), new FezEngine.Readers.DotDialogueLineReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.CameraNodeData), new FezEngine.Readers.CameraNodeDataReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.BackgroundPlane), new FezEngine.Readers.BackgroundPlaneReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.ArtObject), new FezEngine.Readers.ArtObjectReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.ArtObjectInstance), new FezEngine.Readers.ArtObjectInstanceReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.ArtObjectActorSettings), new FezEngine.Readers.ArtObjectActorSettingsReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.AnimatedTexture), new FezEngine.Readers.AnimatedTextureReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.AmbienceTrack), new FezEngine.Readers.AmbienceTrackReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.WinConditions), new FezEngine.Readers.WinConditionsReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.VolumeActorSettings), new FezEngine.Readers.VolumeActorSettingsReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.Geometry.VertexPositionNormalTextureInstance), new FezEngine.Readers.VertexPositionNormalTextureInstanceReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.CollisionType), new Microsoft.Xna.Framework.Content.EnumReader<FezEngine.CollisionType>());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.FaceOrientation), new Microsoft.Xna.Framework.Content.EnumReader<FezEngine.FaceOrientation>());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.SurfaceType), new Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.SurfaceType>());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.ShardNotes), new Microsoft.Xna.Framework.Content.ArrayReader<FezEngine.Structure.ShardNotes>());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.AssembleChords), new Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.AssembleChords>());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.LiquidType), new Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.LiquidType>());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.TrackedSong), new FezEngine.Readers.TrackedSongReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.Scripting.Script), new FezEngine.Readers.ScriptReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.Scripting.ScriptTrigger), new FezEngine.Readers.ScriptTriggerReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.Scripting.ScriptAction), new FezEngine.Readers.ScriptActionReader());
		ContentTypeReaderManager.AddCustomReader(typeof(FezEngine.Structure.Scripting.ScriptCondition), new FezEngine.Readers.ScriptConditionReader());

		Logger.Log("WASM", "Custom Content Readers added.");
	}

	// [DllImport("Emscripten")]
	// public extern static int mount_opfs();

	private static Fez fez;
	public static bool firstLaunch = true;


	[JSExport]
	internal static Task PreInit()
	{
		return Task.Run(() =>
		{
			Console.WriteLine("PreInit");
			// Console.WriteLine("calling mount_opfs");
			// int ret = mount_opfs();
			// Console.WriteLine($"called mount_opfs: {ret}");
			// if (ret != 0)
			// {
			//     throw new Exception("Failed to mount OPFS");
			// }
		});
	}

	[JSExport]
	internal static void Init()
	{
		try
		{
			// Any init for the Game - usually before game.Run() in the decompilation
			Logger.Log("FEZ", "Initializing.");
			Environment.SetEnvironmentVariable("SDL_AUDIO_CHANNELS", "2");
			Environment.SetEnvironmentVariable("SDL_AUDIO_FREQUENCY", "44100");
			Logger.Clear();
			Logger.Log("FEZ", "Past Logger.Clear()");
			// FNALoggerEXT.LogInfo = delegate(string msg)
			// {
			// 	Logger.Log("FNA", LogSeverity.Information, msg);
			// };
			// FNALoggerEXT.LogWarn = delegate(string msg)
			// {
			// 	Logger.Log("FNA", LogSeverity.Warning, msg);
			// };
			// FNALoggerEXT.LogError = delegate(string msg)
			// {
			// 	Logger.Log("FNA", LogSeverity.Error, msg);
			// };
			Logger.Log("FEZ", "Initializing settings.");
			SettingsManager.InitializeSettings();
			Logger.Log("FEZ", "Settings initialized.");
			PersistentThreadPool.SingleThreaded = true; // SettingsManager.Settings.Singlethreaded;
			Logger.Log("FEZ", "Set SingleThreaded.");
			Fez.DisableSteamworksInit = true; // SettingsManager.Settings.DisableSteamworks;
			Logger.Log("FEZ", "Disabled Steamworks.");
			// Queue<string> queue = new Queue<string>();
			// foreach (string item in args)
			// {
			// 	queue.Enqueue(item);
			// }
			// while (queue.Count > 0)
			// {
			// 	switch (queue.Dequeue().ToLower(CultureInfo.InvariantCulture))
			// 	{
			// 	case "-c":
			// 	case "--clear-save-file":
			// 	{
			// 		string text3 = Path.Combine(Util.LocalSaveFolder, "SaveSlot");
			// 		if (File.Exists(text3 + "0"))
			// 		{
			// 			File.Delete(text3 + "0");
			// 		}
			// 		break;
			// 	}
			// 	case "-ng":
			// 	case "--no-gamepad":
			// 		SettingsManager.Settings.DisableController = true;
			// 		break;
			// 	case "-r":
			// 	case "--region":
			// 		SettingsManager.Settings.Language = (Language)Enum.Parse(typeof(Language), queue.Dequeue());
			// 		break;
			// 	case "--trace":
			// 		TraceFlags.TraceContentLoad = true;
			// 		break;
			// 	case "-pd":
			// 	case "--public-demo":
			// 	{
					// Fez.PublicDemo = true;
			// 		string text2 = Path.Combine(Util.LocalSaveFolder, "SaveSlot");
			// 		for (int j = -1; j < 3; j++)
			// 		{
			// 			if (File.Exists(text2 + j))
			// 			{
			// 				File.Delete(text2 + j);
			// 			}
			// 		}
			// 		break;
			// 	}
			// 	case "-nm":
			// 	case "--no-music":
					Fez.NoMusic = true;
			// 		break;
			// 	case "-st":
			// 	case "--singlethreaded":
					SettingsManager.Settings.Singlethreaded = true;
					PersistentThreadPool.SingleThreaded = true;
			// 		break;
			// 	case "--msaa-option":
			// 		SettingsManager.Settings.MultiSampleOption = true;
			// 		break;
			// 	case "--attempt-highdpi":
			// 		SettingsManager.Settings.HighDPI = true;
			// 		break;
			// 	case "--gotta-gomez-fast":
			// 		Fez.SpeedRunMode = true;
			// 		PersistentThreadPool.SingleThreaded = true;
			// 		break;
			// 	case "--mgvs":
			// 	{
			// 		Fez.SkipIntro = true;
			// 		Fez.ForcedLevelName = "VILLAGEVILLE_3D";
			// 		DotService.DisableDotScripting = true;
			// 		GameLevelManager.ResetCubeBitsOnLevelChange = true;
			// 		string text = Path.Combine(Util.LocalSaveFolder, "SaveSlot");
			// 		if (File.Exists(text + "0"))
			// 		{
			// 			File.Delete(text + "0");
			// 		}
			// 		Fez.ForceGoldenCubes = 1;
			// 		TimeHost.FreezeTime = true;
			// 		PersistentThreadPool.SingleThreaded = true;
			// 		break;
			// 	}
			// 	case "--give-bugs-pls":
			// 		Fez.SpeedRunBugs = true;
			// 		break;
			// 	}
			// }
			if (SettingsManager.Settings.HighDPI)
			{
				Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");
			}
			Logger.Log("FEZ", "Initializing audio.");
			try
			{
				SoundEffect.MasterVolume = SoundEffect.MasterVolume;
				Logger.Log("FEZ", "Initialized audio.");
			}
			catch
			{
				// SDL.SDL_SetHint("SDL_AUDIODRIVER", "dummy");
				Logger.Log("FEZ", LogSeverity.Error, "Failed to initialize audio.");
			}
			Logger.Log("FEZ", "Setting thread priority.");
			// Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
			Logger.Log("FEZ", "Instantiating Game.");
			fez = new Fez();

			// SetMainLoop(MainLoop);

			// if (!fez.IsDisposed)
			// {
			// 	Logger.Log("FEZ", "Running Game.");
			// 	fez.Run();
			// }

			// Logger.Try(MainInternal);
			// if (fez != null)
			// {
			// 	fez.Dispose();
			// }
			// fez = null;
			// Logger.Log("FEZ", "Exiting.");
		}
		catch (Exception e)
		{
			Logger.Log("FEZ", LogSeverity.Error, "Error in Init()");
			Logger.LogError(e);
		}
	}

	[JSExport]
	internal static void Cleanup()
	{
		Logger.Log("FEZ", "Cleaning up.");
		// Any cleanup for the Game - usually after game.Run() in the decompilation
	}

	[JSExport]
	internal static bool MainLoop()
	{
		try
		{
			fez.RunOneFrame();
			return true;
		}
		catch (Exception e)
		{
			Logger.Log("FEZ", LogSeverity.Error, "Error in MainLoop()!");
			Logger.LogError(e);
			throw;
		}
		// return fez.RunApplication;
	}

	// [JSImport("setMainLoop", "main.js")]
	// internal static partial void SetMainLoop([JSMarshalAs<JSType.Function>] Action cb);

	// [JSImport("stopMainLoop", "main.js")]
	// internal static partial void StopMainLoop();

	// [JSImport("syncFs", "main.js")]
	// internal static partial void Sync([JSMarshalAs<JSType.Function>] Action cb);
}