using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JiME
{
	public class InteractionConverter : JsonConverter
	{
		public override bool CanWrite => false;
		public override bool CanRead => true;
		public override bool CanConvert( Type objectType )
		{
			return objectType == typeof( IInteraction );
		}

		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			throw new InvalidOperationException( "Use default serialization." );
			//throw new NotImplementedException();
		}

		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			var jsonObject = JArray.Load( reader );
			var interaction = default( IInteraction );
			List<IInteraction> interactionList = new List<IInteraction>();

			foreach ( var item in jsonObject )
			{
				switch ( item["interactionType"].Value<int>() )
				{
					case 0:
						interaction = item.ToObject<TextInteraction>();
						break;
					case 1:
						interaction = item.ToObject<ThreatInteraction>();
						break;
					case 2:
						interaction = item.ToObject<TestInteraction>();
						break;
					case 3:
						interaction = item.ToObject<DecisionInteraction>();
						break;
					case 4:
						interaction = item.ToObject<BranchInteraction>();
						break;
					case 5:
						interaction = item.ToObject<DarknessInteraction>();
						break;
					case 6:
						interaction = item.ToObject<MultiEventInteraction>();
						break;
					case 7:
						interaction = item.ToObject<PersistentTokenInteraction>();
						break;
					case 8:
						interaction = item.ToObject<ConditionalInteraction>();
						break;
				}
				interactionList.Add( interaction );
			}

			return interactionList;
		}

	}
}
