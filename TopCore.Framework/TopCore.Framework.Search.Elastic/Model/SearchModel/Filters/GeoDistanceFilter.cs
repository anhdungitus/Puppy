﻿using TopCore.Framework.Search.Elastic.Model.GeoModel;
using TopCore.Framework.Search.Elastic.Model.Units;
using TopCore.Framework.Search.Elastic.Utils;

namespace TopCore.Framework.Search.Elastic.Model.SearchModel.Filters
{
    /// <summary>
    ///   Filters documents that include only hits that exists within a specific distance from a geo point. 
    /// </summary>
    public class GeoDistanceFilter : IFilter
    {
        private readonly DistanceUnit _distance;
        private readonly string _field;
        private readonly GeoPoint _location;
        private DistanceType _distanceType;
        private bool _distanceTypeSet;
        private OptimizeBbox _optimizeBbox;
        private bool _optimizeBboxSet;

        /// <summary>
        ///   Filters documents that include only hits that exists within a specific distance from a geo point. 
        /// </summary>
        /// <param name="field">    name of the field used for the geo point </param>
        /// <param name="location"> GeoPoint location </param>
        /// <param name="distance">
        ///   The radius of the circle centred on the specified location. Points which fall into this circle are considered to be matches. The distance
        ///   can be specified in various units. See the section called “Distance Unitsedit”.
        /// </param>
        public GeoDistanceFilter(string field, GeoPoint location, DistanceUnit distance)
        {
            _field = field;
            _location = location;
            _distance = distance;
        }

        /// <summary>
        ///   distance_type How to compute the distance. Can either be sloppy_arc (default), arc (slighly more precise but significantly
        ///   slower) or plane (faster, but inaccurate on long distances and close to the poles).
        /// </summary>
        public DistanceType DistanceType
        {
            get => _distanceType;
            set
            {
                _distanceType = value;
                _distanceTypeSet = true;
            }
        }

        /// <summary>
        ///   optimize_bbox Whether to use the optimization of first running a bounding box check before the distance check. Defaults to memory which
        ///   will do in memory checks. Can also have values of indexed to use indexed value check (make sure the geo_point type index lat lon in this
        ///   case), or none which disables bounding box optimization.
        /// </summary>
        public OptimizeBbox OptimizeBbox
        {
            get => _optimizeBbox;
            set
            {
                _optimizeBbox = value;
                _optimizeBboxSet = true;
            }
        }

        public void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            elasticCrudJsonWriter.JsonWriter.WritePropertyName("geo_distance");
            elasticCrudJsonWriter.JsonWriter.WriteStartObject();

            elasticCrudJsonWriter.JsonWriter.WritePropertyName(_field);
            _location.WriteJson(elasticCrudJsonWriter);

            JsonHelper.WriteValue("distance", _distance.GetDistanceUnit(), elasticCrudJsonWriter);
            JsonHelper.WriteValue("distance_type", _distanceType.ToString(), elasticCrudJsonWriter, _distanceTypeSet);
            JsonHelper.WriteValue("optimize_bbox", _optimizeBbox.ToString(), elasticCrudJsonWriter, _optimizeBboxSet);

            elasticCrudJsonWriter.JsonWriter.WriteEndObject();
        }
    }

    public enum OptimizeBbox
    {
        memory,
        indexed,
        none
    }
}