import { useState, useEffect } from 'react'
import { PieChart, Pie, Cell, Tooltip, Legend, ResponsiveContainer } from 'recharts';

// --- COMPONENTES UI (SE DEFINEN AFUERA DE APP PARA EVITAR EL ERROR DE FOCO) ---

const InputDark = (props) => (
  <input {...props} className="w-full bg-slate-800 border border-slate-700 text-white p-3 rounded-lg focus:outline-none focus:ring-2 focus:ring-emerald-500 placeholder-gray-500 transition" />
);

const SelectDark = (props) => (
  <select {...props} className="w-full bg-slate-800 border border-slate-700 text-white p-3 rounded-lg focus:outline-none focus:ring-2 focus:ring-emerald-500 transition">
    {props.children}
  </select>
);

const ButtonAction = ({ children, onClick, color, className }) => {
  const colors = {
    primary: "bg-emerald-600 hover:bg-emerald-500 text-white",
    danger: "bg-rose-600 hover:bg-rose-500 text-white",
    edit: "text-sky-400 hover:text-sky-300",
    delete: "text-rose-400 hover:text-rose-300",
    secondary: "bg-slate-600 hover:bg-slate-500 text-white"
  };
  // Agregamos className opcional por si acaso
  return <button onClick={onClick} className={`px-4 py-2 rounded-lg font-medium transition ${colors[color] || colors.primary} ${className || ''}`}>{children}</button>
};

const Card = ({ children, title }) => (
  <div className="bg-slate-900 border border-slate-800 rounded-2xl shadow-xl overflow-hidden h-full">
    {title && <div className="bg-slate-800/50 p-4 border-b border-slate-800"><h2 className="text-lg font-bold text-gray-100">{title}</h2></div>}
    <div className="p-6">{children}</div>
  </div>
);

// SearchBar necesita recibir el estado desde afuera ahora
const SearchBar = ({ busquedaId, setBusquedaId, onSearch, reload }) => (
  <div className="flex gap-2 mb-6 items-center bg-slate-800 p-2 rounded-xl border border-slate-700 shadow-sm">
    <span className="text-gray-400 text-sm ml-2">🔍 ID:</span>
    <input 
      type="number" 
      placeholder="#" 
      className="bg-slate-900 border border-slate-700 text-white p-1 rounded w-24 text-center focus:outline-none focus:border-emerald-500" 
      value={busquedaId} 
      onChange={(e) => setBusquedaId(e.target.value)} 
    />
    <button onClick={onSearch} className="bg-emerald-600 text-white px-4 py-1 rounded hover:bg-emerald-500 text-sm font-bold">Buscar</button>
    <button onClick={() => { setBusquedaId(""); reload(); }} className="bg-slate-700 text-gray-300 px-4 py-1 rounded hover:bg-slate-600 text-sm" title="Recargar">🔄 Todos</button>
  </div>
);


function App() {
  const PUERTO = "5258"; 
  const API_URL = `http://localhost:${PUERTO}/api`;

  // --- ESTADOS ---
  const [tabActual, setTabActual] = useState("inicio");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [idEditando, setIdEditando] = useState(null);
  const [busquedaId, setBusquedaId] = useState("");

  // DATOS
  const [categorias, setCategorias] = useState([]);
  const [movimientos, setMovimientos] = useState([]);
  const [objetivos, setObjetivos] = useState([]);

  // FORMULARIOS
  const [formCategoria, setFormCategoria] = useState({ nombre: "", esIngreso: "true" });
  const [formMovimiento, setFormMovimiento] = useState({ descripcion: "", monto: "", fecha: "", categoriaId: "" });
  const [formObjetivo, setFormObjetivo] = useState({ nombre: "", montoMeta: "", fechaLimite: "" });

  // --- CARGA DE DATOS ---
  useEffect(() => {
    cargarDatos();
    cancelarEdicion();
    setBusquedaId("");
  }, [tabActual]);

  const cargarDatos = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const resCat = await fetch(`${API_URL}/Categorias`);
      const dataCat = await resCat.json();
      setCategorias(dataCat.Data || []);

      const resMov = await fetch(`${API_URL}/Movimientos`);
      const dataMov = await resMov.json();
      setMovimientos(dataMov.Data || []);

      const resObj = await fetch(`${API_URL}/Objetivos`);
      const dataObj = await resObj.json();
      setObjetivos(dataObj.Data || []);
      
    } catch (err) {
      setError("Error de conexión con el servidor.");
    } finally {
      setIsLoading(false);
    }
  };

  // --- BUSCADOR ---
  const buscarPorId = async (endpoint, setDatos) => {
    if (!busquedaId) return alert("Ingresa un ID numérico");
    setIsLoading(true);
    try {
      const res = await fetch(`${API_URL}/${endpoint}/${busquedaId}`);
      if (!res.ok) { alert("❌ No encontrado."); cargarDatos(); return; }
      const data = await res.json();
      if (data.Success && data.Data) { setDatos([data.Data]); } 
      else { alert("No se encontró."); cargarDatos(); }
    } catch (error) { alert("Error en la búsqueda."); } 
    finally { setIsLoading(false); }
  };

  // --- CÁLCULOS GENERALES ---
  const categoriasConTotal = categorias.map(cat => {
    const total = movimientos.filter(m => m.CategoriaId === cat.Id).reduce((sum, m) => sum + m.Monto, 0);
    return { ...cat, total };
  });

  const ultimosMovimientos = [...movimientos].sort((a, b) => new Date(b.Fecha) - new Date(a.Fecha)).slice(0, 5);
  const totalIngresos = movimientos.filter(m => m.Categoria?.TipoCategoria === true).reduce((sum, m) => sum + m.Monto, 0);
  const totalGastos = movimientos.filter(m => m.Categoria?.TipoCategoria === false).reduce((sum, m) => sum + m.Monto, 0);
  const saldoTotal = totalIngresos - totalGastos;

  // --- CÁLCULOS GRÁFICO ---
  const mesActual = new Date().getMonth();
  const anioActual = new Date().getFullYear();
  const gastosMes = movimientos.filter(m => 
    m.Categoria?.TipoCategoria === false && 
    new Date(m.Fecha).getMonth() === mesActual &&
    new Date(m.Fecha).getFullYear() === anioActual
  );
  const datosGrafico = categorias
    .filter(c => c.TipoCategoria === false)
    .map(cat => {
      const total = gastosMes
        .filter(m => m.CategoriaId === cat.Id)
        .reduce((sum, m) => sum + m.Monto, 0);
      return { name: cat.NombreCategoria, value: total };
    })
    .filter(d => d.value > 0);
  const COLORES_GRAFICO = ['#F43F5E', '#3B82F6', '#10B981', '#F59E0B', '#8B5CF6', '#EC4899', '#06B6D4'];

  // --- CRUD ---
  const iniciarEdicion = (item, tipo) => {
    setIdEditando(item.Id);
    if (tipo === 'categoria') {
      setFormCategoria({ nombre: item.NombreCategoria, esIngreso: item.TipoCategoria ? "true" : "false" });
    } else if (tipo === 'movimiento') {
      setFormMovimiento({ descripcion: item.Descripcion, monto: item.Monto, fecha: item.Fecha.split('T')[0], categoriaId: item.CategoriaId });
    } else if (tipo === 'objetivo') {
      setFormObjetivo({ nombre: item.NombreObjetivo, montoMeta: item.MontoMeta, fechaLimite: item.FechaLimite.split('T')[0] });
    }
  };

  const cancelarEdicion = () => {
    setIdEditando(null);
    setFormCategoria({ nombre: "", esIngreso: "true" });
    setFormMovimiento({ descripcion: "", monto: "", fecha: "", categoriaId: "" });
    setFormObjetivo({ nombre: "", montoMeta: "", fechaLimite: "" });
  };

  const procesarEnvio = async (e, endpoint, body, resetForm) => {
    e.preventDefault();
    let url = `${API_URL}/${endpoint}`;
    let method = "POST";
    if (idEditando) { url += `/${idEditando}`; method = "PUT"; body.Id = idEditando; }
    try {
      const res = await fetch(url, { method: method, headers: { "Content-Type": "application/json" }, body: JSON.stringify(body) });
      if (res.ok) { cargarDatos(); cancelarEdicion(); setBusquedaId(""); } 
      else { alert("Error al guardar."); }
    } catch (error) { alert("Error de conexión"); }
  };

  const submitCategoria = (e) => procesarEnvio(e, 'Categorias', { NombreCategoria: formCategoria.nombre, TipoCategoria: formCategoria.esIngreso === "true" }, setFormCategoria);
  const submitMovimiento = (e) => { if(!formMovimiento.categoriaId) return alert("Selecciona categoría"); procesarEnvio(e, 'Movimientos', { Descripcion: formMovimiento.descripcion, Monto: parseFloat(formMovimiento.monto), Fecha: formMovimiento.fecha || new Date().toISOString(), CategoriaId: parseInt(formMovimiento.categoriaId) }, setFormMovimiento); };
  const submitObjetivo = (e) => procesarEnvio(e, 'Objetivos', { NombreObjetivo: formObjetivo.nombre, MontoMeta: parseFloat(formObjetivo.montoMeta), FechaLimite: formObjetivo.fechaLimite }, setFormObjetivo);
  const eliminar = async (endpoint, id) => { if(!confirm("¿Borrar?")) return; await fetch(`${API_URL}/${endpoint}/${id}`, { method: "DELETE" }); cargarDatos(); };

  return (
    <div className="min-h-screen bg-slate-950 font-sans text-gray-200 pb-20 selection:bg-emerald-500 selection:text-white">
      
      <nav className="bg-slate-900/80 backdrop-blur-md border-b border-slate-800 sticky top-0 z-50">
        <div className="max-w-7xl mx-auto px-6">
          <div className="flex items-center justify-between h-20">
            <h1 className="text-2xl font-bold tracking-tight bg-gradient-to-r from-emerald-400 to-cyan-400 bg-clip-text text-transparent cursor-pointer" onClick={() => setTabActual('inicio')}>
              💰 Finanzas<span className="text-gray-500 font-light">PRO</span>
            </h1>
            <div className="flex space-x-1 bg-slate-800 p-1 rounded-xl">
              {['inicio', 'movimientos', 'categorias', 'objetivos'].map(tab => (
                <button key={tab} onClick={() => setTabActual(tab)} 
                  className={`px-4 py-2 rounded-lg text-sm font-medium transition-all ${tabActual===tab ? 'bg-emerald-600 text-white shadow-lg' : 'text-gray-400 hover:text-white hover:bg-slate-700'}`}>
                  {tab.charAt(0).toUpperCase() + tab.slice(1)}
                </button>
              ))}
            </div>
          </div>
        </div>
      </nav>

      <main className="max-w-7xl mx-auto px-6 mt-10">
        {isLoading && <div className="text-center text-emerald-400 mb-6 animate-pulse">Cargando datos...</div>}
        {error && <div className="bg-rose-900/30 text-rose-300 p-4 rounded-xl mb-6 border border-rose-800 flex items-center gap-2">⚠️ {error}</div>}

        {/* --- PANTALLA INICIO --- */}
        {tabActual === 'inicio' && (
          <div className="space-y-10 animate-fade-in">
            {/* Balance Cards */}
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 shadow-2xl relative overflow-hidden group hover:border-emerald-500/50 transition duration-500">
                <div className="absolute top-0 right-0 w-20 h-20 bg-emerald-500/10 rounded-bl-full -mr-4 -mt-4 transition group-hover:bg-emerald-500/20"></div>
                <p className="text-slate-400 text-sm font-bold uppercase tracking-widest">Ingresos</p>
                <p className="text-4xl font-extrabold text-emerald-400 mt-2">+${totalIngresos.toFixed(2)}</p>
              </div>
              <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 shadow-2xl relative overflow-hidden group hover:border-rose-500/50 transition duration-500">
                <div className="absolute top-0 right-0 w-20 h-20 bg-rose-500/10 rounded-bl-full -mr-4 -mt-4 transition group-hover:bg-rose-500/20"></div>
                <p className="text-slate-400 text-sm font-bold uppercase tracking-widest">Gastos</p>
                <p className="text-4xl font-extrabold text-rose-400 mt-2">-${totalGastos.toFixed(2)}</p>
              </div>
              <div className="bg-gradient-to-br from-slate-800 to-slate-900 p-6 rounded-2xl border border-slate-700 shadow-2xl relative overflow-hidden">
                <p className="text-slate-400 text-sm font-bold uppercase tracking-widest">Saldo Total</p>
                <p className={`text-4xl font-extrabold mt-2 ${saldoTotal >= 0 ? 'text-sky-400' : 'text-rose-400'}`}>${saldoTotal.toFixed(2)}</p>
              </div>
            </div>

            {/* Accesos Rápidos */}
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              {[
                { id: 'movimientos', title: 'Gestionar Movimientos', desc: 'Registra gastos e ingresos', color: 'border-emerald-500' },
                { id: 'categorias', title: 'Gestionar Categorías', desc: 'Organiza tus etiquetas', color: 'border-purple-500' },
                { id: 'objetivos', title: 'Gestionar Objetivos', desc: 'Define tus metas de ahorro', color: 'border-sky-500' }
              ].map(item => (
                <button key={item.id} onClick={() => setTabActual(item.id)} className={`bg-slate-900 p-6 rounded-2xl text-left border border-slate-800 hover:bg-slate-800 transition group border-l-4 ${item.color}`}>
                  <h3 className="text-lg font-bold text-gray-100 group-hover:text-white">{item.title} →</h3>
                  <p className="text-slate-400 text-sm mt-1">{item.desc}</p>
                </button>
              ))}
            </div>

            <div className="grid md:grid-cols-2 gap-8">
              
              {/* --- AQUÍ ESTÁ EL GRÁFICO --- */}
              <Card title="📊 Gastos del Mes (Por Categoría)">
                <div className="h-64 flex items-center justify-center">
                  {datosGrafico.length > 0 ? (
                    <ResponsiveContainer width="100%" height="100%">
                      <PieChart>
                        <Pie
                          data={datosGrafico}
                          cx="50%" cy="50%"
                          innerRadius={60}
                          outerRadius={80}
                          paddingAngle={5}
                          dataKey="value"
                        >
                          {datosGrafico.map((entry, index) => (
                            <Cell key={`cell-${index}`} fill={COLORES_GRAFICO[index % COLORES_GRAFICO.length]} stroke="none" />
                          ))}
                        </Pie>
                        <Tooltip 
                          contentStyle={{ backgroundColor: '#1e293b', borderColor: '#334155', borderRadius: '8px', color: '#fff' }} 
                          itemStyle={{ color: '#fff' }}
                        />
                        <Legend verticalAlign="bottom" height={36} iconType="circle" />
                      </PieChart>
                    </ResponsiveContainer>
                  ) : (
                    <p className="text-slate-500 italic">No hay gastos este mes para graficar.</p>
                  )}
                </div>
              </Card>

              {/* Últimos Movimientos */}
              <Card title="⏱️ Últimos Movimientos">
                <div className="space-y-4">
                  {ultimosMovimientos.map(m => (
                    <div key={m.Id} className="flex justify-between items-center border-b border-slate-800 pb-3 last:border-0 last:pb-0">
                      <div>
                        <p className="text-white font-medium">{m.Descripcion}</p>
                        <p className="text-xs text-slate-500">{new Date(m.Fecha).toLocaleDateString()}</p>
                      </div>
                      <span className={`font-bold ${m.Categoria?.TipoCategoria ? 'text-emerald-400' : 'text-rose-400'}`}>
                        {m.Categoria?.TipoCategoria ? '+' : '-'}${m.Monto}
                      </span>
                    </div>
                  ))}
                  {ultimosMovimientos.length === 0 && <p className="text-slate-500 text-center italic">Lista vacía.</p>}
                </div>
              </Card>
            </div>

            {objetivos.length > 0 && (
              <Card title="🚀 Mis Metas">
                <div className="grid sm:grid-cols-2 md:grid-cols-3 gap-4">
                  {objetivos.map(o => (
                    <div key={o.Id} className="bg-slate-800 p-4 rounded-xl border border-slate-700 hover:border-sky-500/50 transition">
                      <div className="flex justify-between items-start mb-2">
                        <h4 className="font-bold text-gray-100">{o.NombreObjetivo}</h4>
                        <span className="text-xs bg-sky-900 text-sky-300 px-2 py-1 rounded">Meta</span>
                      </div>
                      <p className="text-2xl font-bold text-sky-400">${o.MontoMeta}</p>
                      <p className="text-xs text-slate-500 mt-2">Vence: {new Date(o.FechaLimite).toLocaleDateString()}</p>
                    </div>
                  ))}
                </div>
              </Card>
            )}
          </div>
        )}

        {/* --- PESTAÑAS CRUD (CATEGORIAS, MOVIMIENTOS, OBJETIVOS) --- */}
        
        {tabActual === 'categorias' && (
          <div className="grid md:grid-cols-3 gap-8">
            <div className="md:col-span-1">
              <Card title={idEditando ? "✏️ Editar Categoría" : "✨ Nueva Categoría"}>
                <form onSubmit={submitCategoria} className="space-y-4">
                  <InputDark value={formCategoria.nombre} onChange={e=>setFormCategoria({...formCategoria, nombre:e.target.value})} placeholder="Ej: Comida, Sueldo..." required />
                  <SelectDark value={formCategoria.esIngreso} onChange={e=>setFormCategoria({...formCategoria, esIngreso:e.target.value})}><option value="true">🟢 Ingreso</option><option value="false">🔴 Gasto</option></SelectDark>
                  <div className="flex gap-2 pt-2"><ButtonAction onClick={() => {}} color="primary" className="w-full flex-1">{idEditando ? "Actualizar" : "Guardar"}</ButtonAction>{idEditando && <ButtonAction onClick={cancelarEdicion} color="secondary">Cancelar</ButtonAction>}</div>
                </form>
              </Card>
            </div>
            <div className="md:col-span-2"><Card title="Listado de Categorías">
              <SearchBar busquedaId={busquedaId} setBusquedaId={setBusquedaId} onSearch={() => buscarPorId('Categorias', setCategorias)} reload={cargarDatos} />
              <div className="max-h-96 overflow-y-auto pr-2 space-y-2">{categorias.map(c => (<div key={c.Id} className="flex justify-between items-center bg-slate-800 p-3 rounded-lg border border-slate-700"><div className="flex items-center gap-3"><span className="text-xs text-slate-600 font-mono">#{c.Id}</span><span className={`font-medium ${c.TipoCategoria?"text-emerald-400":"text-rose-400"}`}>{c.NombreCategoria}</span></div><div className="flex gap-3"><ButtonAction onClick={()=>iniciarEdicion(c,'categoria')} color="edit">✏️</ButtonAction><ButtonAction onClick={()=>eliminar('Categorias',c.Id)} color="delete">🗑️</ButtonAction></div></div>))}</div></Card></div>
          </div>
        )}

        {tabActual === 'movimientos' && (
          <div className="grid md:grid-cols-3 gap-8">
            <div className="md:col-span-1">
              <Card title={idEditando ? "✏️ Editar Movimiento" : "✨ Nuevo Movimiento"}>
                <form onSubmit={submitMovimiento} className="space-y-4">
                  <InputDark value={formMovimiento.descripcion} onChange={e=>setFormMovimiento({...formMovimiento, descripcion:e.target.value})} placeholder="Descripción" required />
                  <InputDark type="number" step="0.01" value={formMovimiento.monto} onChange={e=>setFormMovimiento({...formMovimiento, monto:e.target.value})} placeholder="Monto ($)" required />
                  <InputDark type="date" value={formMovimiento.fecha} onChange={e=>setFormMovimiento({...formMovimiento, fecha:e.target.value})} required />
                  <SelectDark value={formMovimiento.categoriaId} onChange={e=>setFormMovimiento({...formMovimiento, categoriaId:e.target.value})} required><option value="">Selecciona Categoría...</option>{categorias.map(c => <option key={c.Id} value={c.Id}>{c.NombreCategoria}</option>)}</SelectDark>
                  <div className="flex gap-2 pt-2"><ButtonAction onClick={() => {}} color="primary" className="w-full flex-1">{idEditando ? "Actualizar" : "Registrar"}</ButtonAction>{idEditando && <ButtonAction onClick={cancelarEdicion} color="secondary">Cancelar</ButtonAction>}</div>
                </form>
              </Card>
            </div>
            <div className="md:col-span-2"><Card title="Historial Completo">
              <SearchBar busquedaId={busquedaId} setBusquedaId={setBusquedaId} onSearch={() => buscarPorId('Movimientos', setMovimientos)} reload={cargarDatos} />
              <div className="overflow-x-auto"><table className="w-full text-left border-collapse"><thead><tr className="text-slate-400 border-b border-slate-700 text-sm"><th className="p-3">ID</th><th className="p-3">Fecha</th><th className="p-3">Descripción</th><th className="p-3">Monto</th><th className="p-3">Acciones</th></tr></thead><tbody className="divide-y divide-slate-800">{movimientos.map(m => (<tr key={m.Id} className="hover:bg-slate-800/50 transition"><td className="p-3 text-xs text-slate-600 font-mono">#{m.Id}</td><td className="p-3 text-sm text-slate-300">{new Date(m.Fecha).toLocaleDateString()}</td><td className="p-3 text-gray-200 font-medium">{m.Descripcion}</td><td className={`p-3 font-bold ${m.Categoria?.TipoCategoria ? 'text-emerald-400' : 'text-rose-400'}`}>${m.Monto}</td><td className="p-3 flex gap-2"><ButtonAction onClick={()=>iniciarEdicion(m,'movimiento')} color="edit">✏️</ButtonAction><ButtonAction onClick={()=>eliminar('Movimientos',m.Id)} color="delete">🗑️</ButtonAction></td></tr>))}</tbody></table></div></Card></div>
          </div>
        )}

        {tabActual === 'objetivos' && (
          <div className="grid md:grid-cols-3 gap-8">
            <div className="md:col-span-1">
              <Card title={idEditando ? "✏️ Editar Objetivo" : "🎯 Nuevo Objetivo"}>
                <form onSubmit={submitObjetivo} className="space-y-4">
                  <InputDark value={formObjetivo.nombre} onChange={e=>setFormObjetivo({...formObjetivo, nombre:e.target.value})} placeholder="Nombre de la Meta" required />
                  <InputDark type="number" step="0.01" value={formObjetivo.montoMeta} onChange={e=>setFormObjetivo({...formObjetivo, montoMeta:e.target.value})} placeholder="Monto Meta ($)" required />
                  <InputDark type="date" value={formObjetivo.fechaLimite} onChange={e=>setFormObjetivo({...formObjetivo, fechaLimite:e.target.value})} required />
                  <div className="flex gap-2 pt-2"><ButtonAction onClick={() => {}} color="primary" className="w-full flex-1">{idEditando ? "Actualizar" : "Crear Meta"}</ButtonAction>{idEditando && <ButtonAction onClick={cancelarEdicion} color="secondary">Cancelar</ButtonAction>}</div>
                </form>
              </Card>
            </div>
            <div className="md:col-span-2"><Card title="Mis Objetivos Financieros">
              <SearchBar busquedaId={busquedaId} setBusquedaId={setBusquedaId} onSearch={() => buscarPorId('Objetivos', setObjetivos)} reload={cargarDatos} />
              <div className="space-y-3">{objetivos.map(o => (<div key={o.Id} className="bg-slate-800 p-4 rounded-xl border border-slate-700 flex justify-between items-center group hover:border-sky-500/50 transition"><div><div className="flex items-center gap-3"><span className="text-xs text-slate-600 font-mono">#{o.Id}</span><h3 className="font-bold text-gray-100 text-lg">{o.NombreObjetivo}</h3></div><p className="text-sky-400 font-bold mt-1">${o.MontoMeta} <span className="text-slate-500 font-normal text-xs ml-2">Vence: {new Date(o.FechaLimite).toLocaleDateString()}</span></p></div><div className="flex gap-3 opacity-50 group-hover:opacity-100 transition"><ButtonAction onClick={()=>iniciarEdicion(o,'objetivo')} color="edit">✏️</ButtonAction><ButtonAction onClick={()=>eliminar('Objetivos',o.Id)} color="delete">🗑️</ButtonAction></div></div>))}</div></Card></div>
          </div>
        )}

      </main>
    </div>
  )
}

export default App