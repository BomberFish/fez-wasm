using System;
using System.Collections.Generic;
using FezEngine.Effects;
using FezEngine.Structure;
using FezEngine.Tools;

namespace FezEngine.Services
{
	public interface ILevelMaterializer
	{
		Mesh TrilesMesh { get; }

		Mesh ArtObjectsMesh { get; }

		Mesh StaticPlanesMesh { get; }

		Mesh AnimatedPlanesMesh { get; }

		Mesh NpcMesh { get; }

		TrileEffect TrilesEffect { get; }

		InstancedArtObjectEffect ArtObjectsEffect { get; }

		IEnumerable<Trile> MaterializedTriles { get; }

		List<BackgroundPlane> LevelPlanes { get; }

		List<ArtObjectInstance> LevelArtObjects { get; }

		RenderPass RenderPass { get; set; }

		event Action<TrileInstance> TrileInstanceBatched;

		void CullEverything();

		void InitializeArtObjects();

		void DestroyMaterializers(TrileSet trileSet);

		void RebuildTriles(bool quick);

		void RebuildTriles(TrileSet trileSet, bool quick);

		void RebuildTrile(Trile trile);

		void ClearBatches();

		void AddInstance(TrileInstance instance);

		void RemoveInstance(TrileInstance trileInstance);

		void RebuildInstances();

		void CullInstances();

		void CleanUp();

		void Rowify();

		void UnRowify();

		void UpdateRow(TrileEmplacement oldEmplacement, TrileInstance instance);

		void FreeScreenSpace(int i, int j);

		void FillScreenSpace(int i, int j);

		void CommitBatchesIfNeeded();

		bool CullInstanceOut(TrileInstance toRemove);

		bool CullInstanceOut(TrileInstance toRemove, bool skipUnregister);

		void CullInstanceIn(TrileInstance toAdd);

		void CullInstanceIn(TrileInstance instance, bool forceAdd);

		void CullInstanceInNoRegister(TrileInstance instance);

		void UpdateInstance(TrileInstance instance);

		bool UnregisterViewedInstance(TrileInstance instance);

		TrileMaterializer GetTrileMaterializer(Trile trile);

		void RegisterSatellites();

		void PrepareFullCull();

		void ForceCull();

		void CleanFallbackTriles();
	}
}
