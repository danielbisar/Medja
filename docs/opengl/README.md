# Modern OpenGL

## Render triangle

```
// setup
var vertexBuffer = new []
{
    0,     0.75f, 0, 
    -0.5f,  0.25f, 0, 
     0.5f,  0.25f, 0, 
};

var bufferIds = new int[1];
GL.GenBuffers(1, bufferIds);

// copy data to GPU memory
// hint: size for BufferData must contain the sizeof(float)
GL.BindBuffer(BufferTarget.ArrayBuffer, bufferIds[0]);
GL.BufferData(BufferTarget.ArrayBuffer, vertexBuffer.Length * sizeof(float), vertexBuffer, BufferUsageHint.StaticDraw);

_vaoId = GL.GenVertexArray();
GL.BindVertexArray(_vaoId);

GL.EnableVertexAttribArray(0);
GL.BindBuffer(BufferTarget.ArrayBuffer, bufferIds[0]);
GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

var vertexShader = new OpenGLShader();
vertexShader.Type = ShaderType.VertexShader;
vertexShader.Source = @"#version 420

in vec3 vertex_pos;

void main()
{
    gl_Position = vec4(vertex_pos, 1);
}";

var fragmentShader = new OpenGLShader();
fragmentShader.Type = ShaderType.FragmentShader;
fragmentShader.Source = @"#version 420

out vec4 outColor;

void main()
{
    outColor = vec4(0.5, 0, 0, 1);
}";

_program = new OpenGLProgram(new[] {vertexShader, fragmentShader}, null);



// render
GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
_program.Use();

GL.BindVertexArray(_vaoId);
GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
```

After writing some API the code looks like (the above is kept for documentation purpose)

```
// setup
var vertexBuffer = new []
{
    0,     0.75f, 0, 
    -0.5f,  0.25f, 0, 
     0.5f,  0.25f, 0, 
};

var vbo = new VertexBufferObject();
vbo.ComponentsPerVertex = 3;
vbo.SetData(data);

var vao = new VertexArrayObject();
_vao.AddVertexAttribute(VertexAttributeType.Positions, vbo);

var vertexShader = new OpenGLShader();
vertexShader.Type = ShaderType.VertexShader;
vertexShader.Source = @"#version 420

" + _vao.GetAttributeLayoutCode() + @"

out vec3 outColor;

void main()
{
    gl_Position = vec4(position, 0, 1);
    outColor = vec3(1,1,1);
}";

var fragmentShader = ShaderFactory.CreatePassthroughFragmentShader(3, "outColor");

_program = OpenGLProgram.Create(vertexShader, fragmentShader);


// render
_program.Use();
_vao.Render();
```

# OpenGL 4 camera setup

Different matrices
- model => contains the model position, rotation etc
- view  => defines the camera (position, look at, direction)
- projection  => used to create an orthographic or perspective view

usually each vertex undergoes the following calculation:

projection * view * model * vertex

The transformations get applied from right to left! So first the model, then view, then projection.

So your shader would look something like:

```
...

void main()
{
    gl_Position = projection * view * model * vec4(vertex_pos, 1);
}
```

or, if you object is not really moving you can combine some of the matrices as well.

