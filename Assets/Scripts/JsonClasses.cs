namespace JsonClasses 
{
    public class SceneInference
    {
        public string scene_category;
        public float ambient_temperature;
    }

    public class ObjectInference
    {
        public string category, category_reason;
        public string material;
        public string usage;
        public string size;
        public string size_reason;
    }

    public class MaterialInference
    {
        public float specific_heat;
        public float thermal_conductivity;
        public float mass;
        public float temperature;
    }

    public class ObjectMaterialInference
    {
        public string object_category, object_category_reason;
        public string material_category, material_justification;
        public string size;
        public string size_reason;
        public float heat_capacity;
        public float thermal_conductivity;
        public float mass;
        public float initial_temperature;
    }

    public class ThermObjectProperties
    {
        public string object_category, object_category_justification, material_category, material_justification, heat_source_justification;
        public float heat_capacity, heat_generation_rate, thermal_conductivity, mass, initial_temperature;
        public bool toggleable, initially_on, preserve_temperature;
    }
}