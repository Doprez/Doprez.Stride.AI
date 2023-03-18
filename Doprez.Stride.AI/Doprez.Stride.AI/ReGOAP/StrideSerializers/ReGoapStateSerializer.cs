using ReGoap.Core;
using Stride.Core.Annotations;
using Stride.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doprez.Stride.AI.ReGOAP.StrideSerializers;

/// <summary>
/// Hopefully this will prevent crashing the GameStudio ¯\_(ツ)_/¯
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="W"></typeparam>
public class ReGoapStateSerializer<T, W> : DataSerializer
{
	public override Type SerializationType => typeof(ReGoapState<T, W>);

	public override bool IsBlittable => false;

	public override void PreSerialize(ref object obj, ArchiveMode mode, [NotNull] SerializationStream stream)
	{

	}

	public override void Serialize(ref object obj, ArchiveMode mode, [NotNull] SerializationStream stream)
	{

	}
}
