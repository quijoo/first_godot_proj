shader_type canvas_item;

uniform vec2 start;
uniform vec2 texture_size;
uniform vec2 window_size;
void fragment()
{
	vec2 position = (start + UV * window_size) / texture_size;
	position.y = 1f - position.y;
	COLOR = texture(TEXTURE, position);
}