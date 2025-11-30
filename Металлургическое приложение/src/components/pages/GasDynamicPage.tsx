import { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Input } from '../ui/input';
import { Label } from '../ui/label';
import { Button } from '../ui/button';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs';
import { Plus, Trash2, Wind, Activity, Flame, Gauge, Package, Droplets, ArrowUpDown, TrendingUp, Factory, AlertCircle, Loader2, Save, BookmarkPlus } from 'lucide-react';
import { Separator } from '../ui/separator';
import { ScrollArea } from '../ui/scroll-area';
import { GasDynamicResults } from './GasDynamicResults';
import { gasDynamicService } from '../../services/api.service';
import { Alert, AlertDescription } from '../ui/alert';
import { useCalculationHistory } from '../../hooks/useCalculationHistory';
import { CalculationHistory } from '../CalculationHistory';
import { SaveCalculationDialog } from '../SaveCalculationDialog';

// Типы данных
interface KoksContent {
  minFractionSize: number;
  fractionPercentage: number;
}

interface AglomContent {
  minFractionSize: number;
  fractionPercentage: number;
  porosity: number;
}

interface OkatContent {
  minFractionSize: number;
  fractionPercentage: number;
  porosity: number;
}

interface CompositionParameters {
  fe_chugun: number;
  mn_chugun: number;
  p_chugun: number;
  si_chugun: number;
  s_shlak: number;
  c_chugun: number;
}

interface FuelAndBlastParameters {
  udeln_koks: number;
  c_neletuch: number;
  stepen_pryamogo_vost: number;
  kislorod_dut: number;
  vlazhn_dut: number;
  udeln_prir_gaz: number;
  c_prir_gaz: number;
  h2_prir_gaz: number;
  stepen_vodorod: number;
  stepen_CO: number;
  rashod_dut: number;
  poteri_dut: number;
}

interface FurnaceGeometry {
  diam_gorn: number;
  diam_raspar: number;
  diam_koloshnik: number;
  height_zaplechik: number;
  height_raspar: number;
  height_shahta: number;
  height_koloshnik: number;
  uroven_zasypi: number;
  kolvo_furm: number;
  diam_furm: number;
  dlina_furm: number;
}

interface ThermalAndPressureParameters {
  temp_dut: number;
  teploemk_koks: number;
  temp_koks: number;
  teplota_nepoln_koks: number;
  teplota_nepoln_prir_gaz: number;
  davlen_izb_dut: number;
  davlen_izb_koloshnik_gaz: number;
  temp_koloshnik_gaz: number;
  perepad_niz: number;
  perepad_verh: number;
}

interface MaterialProperties {
  plotn_shlak: number;
  udeln_vyhod_shlak: number;
  massa_koks_kg: number;
  massa_aglo: number;
  massa_okat: number;
  porozn_aglo: number;
  porozn_okat: number;
  poteri_prokalivanie: number;
}

interface ProductionParameters {
  proizvodit_chugun: number;
  udeln_zhelezorud: number;
  udeln_izvest: number;
  stepen_urav_krit: number;
  dolya_okat: number;
}

export function GasDynamicPage() {
  // Состояния для массивов
  const [koksContents, setKoksContents] = useState<KoksContent[]>([
    { minFractionSize: 0, fractionPercentage: 0 }
  ]);
  const [aglomContents, setAglomContents] = useState<AglomContent[]>([
    { minFractionSize: 0, fractionPercentage: 0, porosity: 0 }
  ]);
  const [okatContents, setOkatContents] = useState<OkatContent[]>([
    { minFractionSize: 0, fractionPercentage: 0, porosity: 0 }
  ]);

  // Состояния для параметров доменной печи
  const [composition, setComposition] = useState<CompositionParameters>({
    fe_chugun: 0, mn_chugun: 0, p_chugun: 0, si_chugun: 0, s_shlak: 0, c_chugun: 0
  });
  const [fuelAndBlast, setFuelAndBlast] = useState<FuelAndBlastParameters>({
    udeln_koks: 0, c_neletuch: 0, stepen_pryamogo_vost: 0, kislorod_dut: 0,
    vlazhn_dut: 0, udeln_prir_gaz: 0, c_prir_gaz: 0, h2_prir_gaz: 0,
    stepen_vodorod: 0, stepen_CO: 0, rashod_dut: 0, poteri_dut: 0
  });
  const [geometry, setGeometry] = useState<FurnaceGeometry>({
    diam_gorn: 0, diam_raspar: 0, diam_koloshnik: 0, height_zaplechik: 0,
    height_raspar: 0, height_shahta: 0, height_koloshnik: 0, uroven_zasypi: 0,
    kolvo_furm: 0, diam_furm: 0, dlina_furm: 0
  });
  const [thermalAndPressure, setThermalAndPressure] = useState<ThermalAndPressureParameters>({
    temp_dut: 0, teploemk_koks: 0, temp_koks: 0, teplota_nepoln_koks: 0,
    teplota_nepoln_prir_gaz: 0, davlen_izb_dut: 0, davlen_izb_koloshnik_gaz: 0,
    temp_koloshnik_gaz: 0, perepad_niz: 0, perepad_verh: 0
  });
  const [materials, setMaterials] = useState<MaterialProperties>({
    plotn_shlak: 0, udeln_vyhod_shlak: 0, massa_koks_kg: 0, massa_aglo: 0,
    massa_okat: 0, porozn_aglo: 0, porozn_okat: 0, poteri_prokalivanie: 0
  });
  const [production, setProduction] = useState<ProductionParameters>({
    proizvodit_chugun: 0, udeln_zhelezorud: 0, udeln_izvest: 0,
    stepen_urav_krit: 0, dolya_okat: 0
  });

  // Состояния для результатов и UI
  const [calculationResults, setCalculationResults] = useState<any>(null);
  const [isCalculating, setIsCalculating] = useState(false);
  const [calculationError, setCalculationError] = useState('');
  const [activeTab, setActiveTab] = useState('aglom');
  
  // История расчетов
  const { history, addToHistory, removeFromHistory, clearHistory } = useCalculationHistory('gas-dynamic');
  const [saveDialogOpen, setSaveDialogOpen] = useState(false);

  // Функции для работы с массивами
  const addKoksContent = () => {
    setKoksContents([...koksContents, { minFractionSize: 0, fractionPercentage: 0 }]);
  };

  const removeKoksContent = (index: number) => {
    setKoksContents(koksContents.filter((_, i) => i !== index));
  };

  const updateKoksContent = (index: number, field: keyof KoksContent, value: number) => {
    const updated = [...koksContents];
    updated[index] = { ...updated[index], [field]: value };
    setKoksContents(updated);
  };

  const addAglomContent = () => {
    setAglomContents([...aglomContents, { minFractionSize: 0, fractionPercentage: 0, porosity: 0 }]);
  };

  const removeAglomContent = (index: number) => {
    setAglomContents(aglomContents.filter((_, i) => i !== index));
  };

  const updateAglomContent = (index: number, field: keyof AglomContent, value: number) => {
    const updated = [...aglomContents];
    updated[index] = { ...updated[index], [field]: value };
    setAglomContents(updated);
  };

  const addOkatContent = () => {
    setOkatContents([...okatContents, { minFractionSize: 0, fractionPercentage: 0, porosity: 0 }]);
  };

  const removeOkatContent = (index: number) => {
    setOkatContents(okatContents.filter((_, i) => i !== index));
  };

  const updateOkatContent = (index: number, field: keyof OkatContent, value: number) => {
    const updated = [...okatContents];
    updated[index] = { ...updated[index], [field]: value };
    setOkatContents(updated);
  };

  const handleCalculate = async () => {
    setIsCalculating(true);
    setCalculationError('');
    setCalculationResults(null);

    try {
      const response = await gasDynamicService.calculate({
        koksContents,
        aglomContents,
        okatContents,
        composition,
        fuelAndBlast,
        geometry,
        thermalAndPressure,
        materials,
        production
      });

      setCalculationResults(response.data);
    } catch (error: any) {
      setCalculationError(error.message || 'Произошла ошибка при расчете.');
    } finally {
      setIsCalculating(false);
    }
  };
  
  const generateResultsSummary = (results: any): string => {
    if (!results || !results.blastFurnace) return 'Нет данных';
    
    const bf = results.blastFurnace;
    return `Порозность агл: ${results.aglomOutput?.aglomPorosity?.toFixed(3) || 'N/A'}, окат: ${results.aglomOutput?.okatPorosity?.toFixed(3) || 'N/A'} | Удельный расход кокса: ${bf.materialConsumption?.udeln_Koks_1000?.toFixed(2) || 'N/A'} т/т | Выход колошникового газа: ${bf.topGas?.udeln_Kolgaz?.toFixed(1) || 'N/A'} м³/т | Перепад давления: ${bf.hydrodynamicsLower?.perepad_Niz_Itog?.toFixed(3) || 'N/A'} атм`;
  };
  
  const handleSaveToHistory = (note: string) => {
    if (calculationResults) {
      const summary = generateResultsSummary(calculationResults);
      addToHistory(note, summary);
    }
  };

  return (
    <div className="space-y-6">
      <div>
        <div className="flex items-center gap-3 mb-2">
          <Wind className="size-8 text-primary" />
          <h1 className="text-3xl">Газодинамический режим доменной плавки</h1>
        </div>
        <p className="text-muted-foreground">
          Расчет газодинамических параметров доменного процесса
        </p>
      </div>

      {calculationError && (
        <Alert variant="destructive">
          <AlertCircle className="size-4" />
          <AlertDescription>{calculationError}</AlertDescription>
        </Alert>
      )}

      <div className="grid gap-6 lg:grid-cols-[1fr_380px]">
        {/* Основной контент */}
        <div>
          <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
            <TabsList className="grid w-full grid-cols-3">
              <TabsTrigger value="aglom">Агломерат и кокс</TabsTrigger>
              <TabsTrigger value="blast-furnace">Параметры печи</TabsTrigger>
              <TabsTrigger value="results">Результаты</TabsTrigger>
            </TabsList>

            {/* Вкладка 1: Агломерат и кокс */}
            <TabsContent value="aglom" className="space-y-6">
              {/* Кокс */}
              <Card>
                <CardHeader>
                  <CardTitle>Фракционный состав кокса</CardTitle>
                  <CardDescription>
                    Распределение фракций кокса по размерам
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  {koksContents.map((koks, index) => (
                    <div key={index} className="flex gap-4 items-end p-4 border border-border rounded-lg">
                      <div className="flex-1 space-y-2">
                        <Label>Минимальный размер фракции, мм</Label>
                        <Input
                          type="number"
                          value={koks.minFractionSize}
                          onChange={(e) => updateKoksContent(index, 'minFractionSize', parseFloat(e.target.value) || 0)}
                        />
                      </div>
                      <div className="flex-1 space-y-2">
                        <Label>Содержание фракции, %</Label>
                        <Input
                          type="number"
                          value={koks.fractionPercentage}
                          onChange={(e) => updateKoksContent(index, 'fractionPercentage', parseFloat(e.target.value) || 0)}
                        />
                      </div>
                      <Button
                        variant="destructive"
                        size="icon"
                        onClick={() => removeKoksContent(index)}
                        disabled={koksContents.length === 1}
                      >
                        <Trash2 className="size-4" />
                      </Button>
                    </div>
                  ))}
                  <Button onClick={addKoksContent} variant="outline" className="w-full">
                    <Plus className="size-4 mr-2" />
                    Добавить фракцию кокса
                  </Button>
                </CardContent>
              </Card>

              {/* Агломерат */}
              <Card>
                <CardHeader>
                  <CardTitle>Фракционный состав агломерата</CardTitle>
                  <CardDescription>
                    Распределение фракций агломерата по размерам и пористость
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  {aglomContents.map((aglom, index) => (
                    <div key={index} className="flex gap-4 items-end p-4 border border-border rounded-lg">
                      <div className="flex-1 space-y-2">
                        <Label>Минимальный размер фракции, мм</Label>
                        <Input
                          type="number"
                          value={aglom.minFractionSize}
                          onChange={(e) => updateAglomContent(index, 'minFractionSize', parseFloat(e.target.value) || 0)}
                        />
                      </div>
                      <div className="flex-1 space-y-2">
                        <Label>Содержание фракции, %</Label>
                        <Input
                          type="number"
                          value={aglom.fractionPercentage}
                          onChange={(e) => updateAglomContent(index, 'fractionPercentage', parseFloat(e.target.value) || 0)}
                        />
                      </div>
                      <div className="flex-1 space-y-2">
                        <Label>Пористость, м³/м³</Label>
                        <Input
                          type="number"
                          value={aglom.porosity}
                          onChange={(e) => updateAglomContent(index, 'porosity', parseFloat(e.target.value) || 0)}
                        />
                      </div>
                      <Button
                        variant="destructive"
                        size="icon"
                        onClick={() => removeAglomContent(index)}
                        disabled={aglomContents.length === 1}
                      >
                        <Trash2 className="size-4" />
                      </Button>
                    </div>
                  ))}
                  <Button onClick={addAglomContent} variant="outline" className="w-full">
                    <Plus className="size-4 mr-2" />
                    Добавить фракцию агломерата
                  </Button>
                </CardContent>
              </Card>

              {/* Окатыши */}
              <Card>
                <CardHeader>
                  <CardTitle>Фракционный состав окатышей</CardTitle>
                  <CardDescription>
                    Распределение фракций окатышей по размерам и пористость
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  {okatContents.map((okat, index) => (
                    <div key={index} className="flex gap-4 items-end p-4 border border-border rounded-lg">
                      <div className="flex-1 space-y-2">
                        <Label>Минимальный размер фракции, мм</Label>
                        <Input
                          type="number"
                          value={okat.minFractionSize}
                          onChange={(e) => updateOkatContent(index, 'minFractionSize', parseFloat(e.target.value) || 0)}
                        />
                      </div>
                      <div className="flex-1 space-y-2">
                        <Label>Содержание фракции, %</Label>
                        <Input
                          type="number"
                          value={okat.fractionPercentage}
                          onChange={(e) => updateOkatContent(index, 'fractionPercentage', parseFloat(e.target.value) || 0)}
                        />
                      </div>
                      <div className="flex-1 space-y-2">
                        <Label>Пористость, м³/м³</Label>
                        <Input
                          type="number"
                          value={okat.porosity}
                          onChange={(e) => updateOkatContent(index, 'porosity', parseFloat(e.target.value) || 0)}
                        />
                      </div>
                      <Button
                        variant="destructive"
                        size="icon"
                        onClick={() => removeOkatContent(index)}
                        disabled={okatContents.length === 1}
                      >
                        <Trash2 className="size-4" />
                      </Button>
                    </div>
                  ))}
                  <Button onClick={addOkatContent} variant="outline" className="w-full">
                    <Plus className="size-4 mr-2" />
                    Добавить фракцию окатышей
                  </Button>
                </CardContent>
              </Card>
            </TabsContent>

            {/* Вкладка 2: Параметры доменной печи */}
            <TabsContent value="blast-furnace" className="space-y-6">
              {/* Состав чугуна и шлака */}
              <Card>
                <CardHeader>
                  <CardTitle>Состав чугуна и шлака</CardTitle>
                  <CardDescription>
                    Химический состав целевого чугуна
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label>Содержание железа [Fe], %</Label>
                      <Input
                        type="number"
                        value={composition.fe_chugun}
                        onChange={(e) => setComposition({...composition, fe_chugun: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Содержание марганца [Mn], %</Label>
                      <Input
                        type="number"
                        value={composition.mn_chugun}
                        onChange={(e) => setComposition({...composition, mn_chugun: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Содержание фосфора [P], %</Label>
                      <Input
                        type="number"
                        value={composition.p_chugun}
                        onChange={(e) => setComposition({...composition, p_chugun: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Содержание кремния [Si], %</Label>
                      <Input
                        type="number"
                        value={composition.si_chugun}
                        onChange={(e) => setComposition({...composition, si_chugun: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Содержание серы (S), %</Label>
                      <Input
                        type="number"
                        value={composition.s_shlak}
                        onChange={(e) => setComposition({...composition, s_shlak: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Содержание углерода [C], %</Label>
                      <Input
                        type="number"
                        value={composition.c_chugun}
                        onChange={(e) => setComposition({...composition, c_chugun: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                  </div>
                </CardContent>
              </Card>

              {/* Топливо и дутье */}
              <Card>
                <CardHeader>
                  <CardTitle>Параметры топлива и дутья</CardTitle>
                  <CardDescription>
                    Характеристики кокса, природного газа и дутья
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label>Удельный расход кокса, кг/т чугуна</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.udeln_koks}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, udeln_koks: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Содержание нелетучего углерода в коксе, %</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.c_neletuch}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, c_neletuch: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Степень прямого восстановления (rd)</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.stepen_pryamogo_vost}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, stepen_pryamogo_vost: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Содержание кислорода в дутье, %</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.kislorod_dut}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, kislorod_dut: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Влажность дутья, г/м³</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.vlazhn_dut}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, vlazhn_dut: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Удельный расход природного газа, м³/т чугуна</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.udeln_prir_gaz}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, udeln_prir_gaz: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Содержание C в природном газе (CH₄), м³/м³</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.c_prir_gaz}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, c_prir_gaz: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Содержание H₂ в природном газе (2CH₄), м³/м³</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.h2_prir_gaz}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, h2_prir_gaz: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Степень использования водорода (nH₂)</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.stepen_vodorod}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, stepen_vodorod: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Степень использования CO (nCO)</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.stepen_CO}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, stepen_CO: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Минутный расход дутья, м³/мин</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.rashod_dut}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, rashod_dut: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Потери давления горячего дутья, %</Label>
                      <Input
                        type="number"
                        value={fuelAndBlast.poteri_dut}
                        onChange={(e) => setFuelAndBlast({...fuelAndBlast, poteri_dut: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                  </div>
                </CardContent>
              </Card>

              {/* Геометрия печи */}
              <Card>
                <CardHeader>
                  <CardTitle>Геометрия доменной печи</CardTitle>
                  <CardDescription>
                    Размеры и конструктивные параметры печи
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label>Диаметр горна, м</Label>
                      <Input
                        type="number"
                        value={geometry.diam_gorn}
                        onChange={(e) => setGeometry({...geometry, diam_gorn: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Диаметр распара, м</Label>
                      <Input
                        type="number"
                        value={geometry.diam_raspar}
                        onChange={(e) => setGeometry({...geometry, diam_raspar: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Диаметр колошника, м</Label>
                      <Input
                        type="number"
                        value={geometry.diam_koloshnik}
                        onChange={(e) => setGeometry({...geometry, diam_koloshnik: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Высота заплечиков, м</Label>
                      <Input
                        type="number"
                        value={geometry.height_zaplechik}
                        onChange={(e) => setGeometry({...geometry, height_zaplechik: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Высота распара, м</Label>
                      <Input
                        type="number"
                        value={geometry.height_raspar}
                        onChange={(e) => setGeometry({...geometry, height_raspar: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Высота шахты, м</Label>
                      <Input
                        type="number"
                        value={geometry.height_shahta}
                        onChange={(e) => setGeometry({...geometry, height_shahta: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Высота колошника, м</Label>
                      <Input
                        type="number"
                        value={geometry.height_koloshnik}
                        onChange={(e) => setGeometry({...geometry, height_koloshnik: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Уровень засыпи, м</Label>
                      <Input
                        type="number"
                        value={geometry.uroven_zasypi}
                        onChange={(e) => setGeometry({...geometry, uroven_zasypi: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Число работающих воздушных фурм, шт</Label>
                      <Input
                        type="number"
                        value={geometry.kolvo_furm}
                        onChange={(e) => setGeometry({...geometry, kolvo_furm: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Диаметр воздушных фурм, мм</Label>
                      <Input
                        type="number"
                        value={geometry.diam_furm}
                        onChange={(e) => setGeometry({...geometry, diam_furm: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Длина фурмы, мм</Label>
                      <Input
                        type="number"
                        value={geometry.dlina_furm}
                        onChange={(e) => setGeometry({...geometry, dlina_furm: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                  </div>
                </CardContent>
              </Card>

              {/* Тепловые и давление */}
              <Card>
                <CardHeader>
                  <CardTitle>Тепловые параметры и давление</CardTitle>
                  <CardDescription>
                    Температурные режимы и давление в печи
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label>Температура горячего дутья, °C</Label>
                      <Input
                        type="number"
                        value={thermalAndPressure.temp_dut}
                        onChange={(e) => setThermalAndPressure({...thermalAndPressure, temp_dut: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Теплоёмкость кокса, кДж/(кг·К)</Label>
                      <Input
                        type="number"
                        value={thermalAndPressure.teploemk_koks}
                        onChange={(e) => setThermalAndPressure({...thermalAndPressure, teploemk_koks: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Температура кокса у фурм, °C</Label>
                      <Input
                        type="number"
                        value={thermalAndPressure.temp_koks}
                        onChange={(e) => setThermalAndPressure({...thermalAndPressure, temp_koks: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Теплота неполного горения С кокса, кДж/кг</Label>
                      <Input
                        type="number"
                        value={thermalAndPressure.teplota_nepoln_koks}
                        onChange={(e) => setThermalAndPressure({...thermalAndPressure, teplota_nepoln_koks: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Теплота неполного горения природного газа, кДж/м³</Label>
                      <Input
                        type="number"
                        value={thermalAndPressure.teplota_nepoln_prir_gaz}
                        onChange={(e) => setThermalAndPressure({...thermalAndPressure, teplota_nepoln_prir_gaz: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Избыточное давление горячего дутья, ати</Label>
                      <Input
                        type="number"
                        value={thermalAndPressure.davlen_izb_dut}
                        onChange={(e) => setThermalAndPressure({...thermalAndPressure, davlen_izb_dut: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Избыточное давление колошникового газа, ати</Label>
                      <Input
                        type="number"
                        value={thermalAndPressure.davlen_izb_koloshnik_gaz}
                        onChange={(e) => setThermalAndPressure({...thermalAndPressure, davlen_izb_koloshnik_gaz: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Температура колошникового газа, °C</Label>
                      <Input
                        type="number"
                        value={thermalAndPressure.temp_koloshnik_gaz}
                        onChange={(e) => setThermalAndPressure({...thermalAndPressure, temp_koloshnik_gaz: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Нижний перепад давления (измеренный), атм</Label>
                      <Input
                        type="number"
                        value={thermalAndPressure.perepad_niz}
                        onChange={(e) => setThermalAndPressure({...thermalAndPressure, perepad_niz: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Верхний перепад давления (измеренный), атм</Label>
                      <Input
                        type="number"
                        value={thermalAndPressure.perepad_verh}
                        onChange={(e) => setThermalAndPressure({...thermalAndPressure, perepad_verh: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                  </div>
                </CardContent>
              </Card>

              {/* Свойства материалов */}
              <Card>
                <CardHeader>
                  <CardTitle>Свойства материалов</CardTitle>
                  <CardDescription>
                    Физические свойства шихтовых материалов
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label>Плотность шлака, кг/м³</Label>
                      <Input
                        type="number"
                        value={materials.plotn_shlak}
                        onChange={(e) => setMaterials({...materials, plotn_shlak: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Удельный выход шлака, кг/т чугуна</Label>
                      <Input
                        type="number"
                        value={materials.udeln_vyhod_shlak}
                        onChange={(e) => setMaterials({...materials, udeln_vyhod_shlak: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Насыпная масса кокса, кг/м³</Label>
                      <Input
                        type="number"
                        value={materials.massa_koks_kg}
                        onChange={(e) => setMaterials({...materials, massa_koks_kg: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Насыпная масса агломерата, т/м³</Label>
                      <Input
                        type="number"
                        value={materials.massa_aglo}
                        onChange={(e) => setMaterials({...materials, massa_aglo: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Насыпная масса окатышей, т/м³</Label>
                      <Input
                        type="number"
                        value={materials.massa_okat}
                        onChange={(e) => setMaterials({...materials, massa_okat: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Пористость агломерата, м³/м³</Label>
                      <Input
                        type="number"
                        value={materials.porozn_aglo}
                        onChange={(e) => setMaterials({...materials, porozn_aglo: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Пористость окатышей, м³/м³</Label>
                      <Input
                        type="number"
                        value={materials.porozn_okat}
                        onChange={(e) => setMaterials({...materials, porozn_okat: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Потеря массы при прокаливании, %</Label>
                      <Input
                        type="number"
                        value={materials.poteri_prokalivanie}
                        onChange={(e) => setMaterials({...materials, poteri_prokalivanie: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                  </div>
                </CardContent>
              </Card>

              {/* Производственные параметры */}
              <Card>
                <CardHeader>
                  <CardTitle>Производственные параметры</CardTitle>
                  <CardDescription>
                    Производительность и расходы материалов
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label>Суточная производительность по чугуну, т/сут</Label>
                      <Input
                        type="number"
                        value={production.proizvodit_chugun}
                        onChange={(e) => setProduction({...production, proizvodit_chugun: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Удельный расход железорудного материала, т/т чугуна</Label>
                      <Input
                        type="number"
                        value={production.udeln_zhelezorud}
                        onChange={(e) => setProduction({...production, udeln_zhelezorud: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Удельный расход известняка, кг/т чугуна</Label>
                      <Input
                        type="number"
                        value={production.udeln_izvest}
                        onChange={(e) => setProduction({...production, udeln_izvest: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Критическая степень уравновешивания шихты, %</Label>
                      <Input
                        type="number"
                        value={production.stepen_urav_krit}
                        onChange={(e) => setProduction({...production, stepen_urav_krit: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Доля окатышей в шихте, %</Label>
                      <Input
                        type="number"
                        value={production.dolya_okat}
                        onChange={(e) => setProduction({...production, dolya_okat: parseFloat(e.target.value) || 0})}
                      />
                    </div>
                  </div>
                </CardContent>
              </Card>

              <div className="flex justify-end gap-4">
                <Button variant="outline" size="lg">Сбросить</Button>
                <Button onClick={handleCalculate} size="lg" disabled={isCalculating}>
                  {isCalculating ? (
                    <>
                      <Loader2 className="size-4 mr-2 animate-spin" />
                      Выполняется расчет...
                    </>
                  ) : (
                    'Выполнить расчет'
                  )}
                </Button>
              </div>
            </TabsContent>

            {/* Вкладка 3: Результаты */}
            <TabsContent value="results" className="space-y-6">
              {calculationResults && (
                <div className="flex justify-end mb-4">
                  <Button onClick={() => setSaveDialogOpen(true)} variant="outline">
                    <BookmarkPlus className="size-4 mr-2" />
                    Сохранить в историю
                  </Button>
                </div>
              )}
              <GasDynamicResults results={calculationResults} />
            </TabsContent>
          </Tabs>
        </div>

        {/* Панель истории расчетов */}
        <div className="hidden lg:block">
          <CalculationHistory
            history={history}
            onRemove={removeFromHistory}
            onClear={clearHistory}
          />
        </div>
      </div>
      
      {/* Диалог сохранения */}
      <SaveCalculationDialog
        open={saveDialogOpen}
        onOpenChange={setSaveDialogOpen}
        onSave={handleSaveToHistory}
        resultsPreview={calculationResults ? generateResultsSummary(calculationResults) : ''}
      />
    </div>
  );
}