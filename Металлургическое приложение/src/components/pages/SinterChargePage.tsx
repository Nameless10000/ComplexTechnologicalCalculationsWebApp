import { useState, useEffect } from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../ui/card";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import { Button } from "../ui/button";
import {
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from "../ui/tabs";
import {
  Plus,
  Trash2,
  Factory,
  Save,
  Calculator,
  ChevronDown,
  ChevronUp,
  Layers,
  Flame,
  Beaker,
  Settings,
  AlertCircle,
} from "lucide-react";
import { Separator } from "../ui/separator";
import { ScrollArea } from "../ui/scroll-area";
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "../ui/collapsible";
import { Alert, AlertDescription } from "../ui/alert";
import { useCalculationHistory } from "../../hooks/useCalculationHistory";
import { CalculationHistory } from "../CalculationHistory";
import { SaveCalculationDialog } from "../SaveCalculationDialog";
import { SinterChargeResults, AglomResponseData, ComponentInfo } from "./SinterChargeResults";
import { aglomModeService } from "../../services/api.service";

// --- Interfaces ---

export interface ZolaOfCocksick {
  fe: number;
  cao: number;
  sio2: number;
  al2o3: number;
  mgo: number;
  p: number;
}

export interface Cocksick {
  weight: number;
  percentZola: number;
  percentSera: number;
  percentValotiles: number;
  percentC: number;
}

export interface FluxAdditions {
  // Izvestnyak
  izvestnyakCaO: number;
  izvestnyakSiO2: number;
  izvestnyakAl2O3: number;
  izvestnyakMgO: number;
  izvestnyakPMPP: number;

  // Dolomyte
  dolomyteCaO: number;
  dolomyteSiO2: number;
  dolomyteAl2O3: number;
  dolomyteMgO: number;
  dolomytePMPP: number;
}

export interface ShihtaComponent {
  id: string; // Internal use for React keys
  // Osnova
  name: string;
  weight: number;
  wet: number;
  pmpp: number;

  // Chemical Components
  fe: number;
  feo: number;
  cao: number;
  sio2: number;
  mgo: number;
  al2o3: number;
  tio2: number;
  s: number;
  p: number;
  cr: number;
  zn: number;
  mno: number;
}

export interface StartEnter {
  osnovnost: number;
  feoInAgl: number;
  dolomyteInAgl: number;
}

export interface AglomRequestData {
  userId: number;
  zolaOfCocksick: ZolaOfCocksick;
  cocksick: Cocksick;
  fluxAdditions: FluxAdditions;
  shihtaComponents: ShihtaComponent[];
  startEnter: StartEnter;
}

// --- Default Values ---

const initialZola: ZolaOfCocksick = {
  fe: 0,
  cao: 0,
  sio2: 0,
  al2o3: 0,
  mgo: 0,
  p: 0,
};

const initialCoke: Cocksick = {
  weight: 0,
  percentZola: 0,
  percentSera: 0,
  percentValotiles: 0,
  percentC: 0,
};

const initialFlux: FluxAdditions = {
  izvestnyakCaO: 0,
  izvestnyakSiO2: 0,
  izvestnyakAl2O3: 0,
  izvestnyakMgO: 0,
  izvestnyakPMPP: 0,
  dolomyteCaO: 0,
  dolomyteSiO2: 0,
  dolomyteAl2O3: 0,
  dolomyteMgO: 0,
  dolomytePMPP: 0,
};

const initialStartEnter: StartEnter = {
  osnovnost: 0,
  feoInAgl: 0,
  dolomyteInAgl: 0,
};

const createEmptyComponent = (): ShihtaComponent => ({
  id: crypto.randomUUID(),
  name: "Новый компонент",
  weight: 0,
  wet: 0,
  pmpp: 0,
  fe: 0,
  feo: 0,
  cao: 0,
  sio2: 0,
  mgo: 0,
  al2o3: 0,
  tio2: 0,
  s: 0,
  p: 0,
  cr: 0,
  zn: 0,
  mno: 0,
});

export function SinterChargePage() {
      useEffect(() => {
        aglomModeService.getPreset()
          .then(({data}) => {
            setStartEnter(data.startEnter);
            setCoke(data.cocksick);
            setZola(data.zolaOfCocksick);
            setFlux(data.fluxAdditions);
            setComponents(data.shihtaComponents);
          })
  }, []);

  const [startEnter, setStartEnter] = useState<StartEnter>(initialStartEnter);
  const [coke, setCoke] = useState<Cocksick>(initialCoke);
  const [zola, setZola] = useState<ZolaOfCocksick>(initialZola);
  const [flux, setFlux] = useState<FluxAdditions>(initialFlux);
  const [components, setComponents] = useState<ShihtaComponent[]>([
    createEmptyComponent(),
  ]);


  // Collapsible state for components
  const [openComponents, setOpenComponents] = useState<Record<string, boolean>>(
    {}
  );
  
  // UI State
  const [activeTab, setActiveTab] = useState("initial-params");
  const [isCalculating, setIsCalculating] = useState(false);
  const [calculationError, setCalculationError] = useState("");
  const [calculationResults, setCalculationResults] = useState<AglomResponseData | null>(null);
  const [saveDialogOpen, setSaveDialogOpen] = useState(false);

  // History Hook
  const { history, addToHistory, removeFromHistory, clearHistory } = useCalculationHistory('sinter-charge');

  const toggleComponent = (id: string) => {
    setOpenComponents((prev) => ({
      ...prev,
      [id]: !prev[id],
    }));
  };

  const handleCalculate = async () => {
    setIsCalculating(true);
    setCalculationError("");
    setCalculationResults(null);

    try {
        const requestData: AglomRequestData = {
            userId: 1,
            startEnter,
            cocksick: coke,
            zolaOfCocksick: zola,
            fluxAdditions: flux,
            shihtaComponents: components,
        };
        console.log("Calculation Request:", requestData);

        // Simulate API delay
        await new Promise((resolve) => setTimeout(resolve, 800));

        // Mock Results Generation based on inputs
        const mockComponents: ComponentInfo[] = components.map(c => ({
            componentName: c.name || "Unnamed",
            reportComponentOfShihta: c.weight * 0.95, // mock logic
            reportFe: c.fe * 1.05,
            reportS: c.s,
            reportP: c.p,
            reportFeO: c.feo,
            reportCaO: c.cao,
            reportSiO2: c.sio2,
            reportAl2O3: c.al2o3,
            reportMgO: c.mgo,
            reportMnO: c.mno,
            reportTiO2: c.tio2,
            reportZn: c.zn,
            reportPMPP: c.pmpp,
            reportFe2O3: (c.fe - c.feo * 0.777) * 1.43, // rough chem logic
            reportOxideSum: c.fe + c.sio2 + c.cao + c.mgo + c.al2o3,
            reportCaO_SiO2: c.sio2 > 0 ? c.cao / c.sio2 : 0,
        }));

        // Add a "Total" row
        const totalWeight = mockComponents.reduce((sum, c) => sum + c.reportComponentOfShihta, 0);
        const totalFe = mockComponents.reduce((sum, c) => sum + c.reportFe * c.reportComponentOfShihta, 0) / totalWeight;

        mockComponents.push({
            componentName: "ИТОГО АГЛОМЕРАТ",
            reportComponentOfShihta: totalWeight,
            reportFe: totalFe,
            reportS: 0.05,
            reportP: 0.08,
            reportFeO: 12.5,
            reportCaO: 10.2,
            reportSiO2: 8.5,
            reportAl2O3: 1.5,
            reportMgO: 2.1,
            reportMnO: 0.5,
            reportTiO2: 0.1,
            reportZn: 0.01,
            reportPMPP: 0,
            reportFe2O3: 75.2,
            reportOxideSum: 95.4,
            reportCaO_SiO2: 1.2, // Target basicity
        });

        const mockResponse: AglomResponseData = {
            components: mockComponents
        };

        setCalculationResults(mockResponse);
        setActiveTab("results");

    } catch (error: any) {
        setCalculationError(error.message || "Произошла ошибка при расчете.");
    } finally {
        setIsCalculating(false);
    }
  };
  
  const generateResultsSummary = (results: AglomResponseData): string => {
      if (!results || !results.components.length) return "Нет данных";
      const total = results.components[results.components.length - 1];
      return `Агломерат: Fe=${total.reportFe.toFixed(2)}%, Основность=${total.reportCaO_SiO2.toFixed(2)}, S=${total.reportS.toFixed(3)}%`;
  };

  const handleSaveToHistory = (note: string) => {
    if (calculationResults) {
      const summary = generateResultsSummary(calculationResults);
      addToHistory(note, summary);
    }
  };

  const addComponent = () => {
    const newComp = createEmptyComponent();
    setComponents([...components, newComp]);
    setOpenComponents((prev) => ({ ...prev, [newComp.id]: true }));
  };

  const removeComponent = (id: string) => {
    setComponents(components.filter((c) => c.id !== id));
  };

  const updateComponent = (
    id: string,
    field: keyof ShihtaComponent,
    value: string | number
  ) => {
    setComponents(
      components.map((c) => (c.id === id ? { ...c, [field]: value } : c))
    );
  };

  // Helper for generic number inputs
  const NumberInput = ({
    value,
    onChange,
    label,
    step = "0.01",
  }: {
    value: number;
    onChange: (val: number) => void;
    label: string;
    step?: string;
  }) => (
    <div className="space-y-1">
      <Label className="text-xs text-muted-foreground">{label}</Label>
      <Input
        type="number"
        step={step}
        value={value}
        onChange={(e) => onChange(parseFloat(e.target.value) || 0)}
        className="h-8"
      />
    </div>
  );

  return (
    <div className="space-y-6 animate-in fade-in duration-500">
      <div>
        <div className="flex items-center gap-3 mb-2">
            <Layers className="size-8 text-primary" />
            <h1 className="text-3xl font-bold tracking-tight">
            Агломерационная шихта
            </h1>
        </div>
        <p className="text-muted-foreground">
          Расчет состава агломерационной шихты и основных показателей
        </p>
      </div>

      {calculationError && (
        <Alert variant="destructive">
          <AlertCircle className="size-4" />
          <AlertDescription>{calculationError}</AlertDescription>
        </Alert>
      )}

      <div className="grid gap-6 lg:grid-cols-[1fr_380px]">
        {/* Main Content */}
        <div>
            <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
                <TabsList className="grid w-full grid-cols-5">
                    <TabsTrigger value="initial-params">Параметры</TabsTrigger>
                    <TabsTrigger value="flux">Флюсы</TabsTrigger>
                    <TabsTrigger value="coke">Кокс</TabsTrigger>
                    <TabsTrigger value="components">Шихта</TabsTrigger>
                    <TabsTrigger value="results">Результаты</TabsTrigger>
                </TabsList>

                {/* Tab 1: Start Parameters */}
                <TabsContent value="initial-params" className="space-y-6">
                    <Card className="border-l-4 border-l-blue-500 shadow-sm">
                        <CardHeader className="pb-3">
                        <CardTitle className="text-lg flex items-center gap-2">
                            <Settings className="h-5 w-5" />
                            Начальные параметры
                        </CardTitle>
                        <CardDescription>Базовые условия для расчета шихты</CardDescription>
                        </CardHeader>
                        <CardContent className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <div className="space-y-2">
                            <Label>Основность</Label>
                            <Input
                            type="number"
                            value={startEnter.osnovnost}
                            onChange={(e) =>
                                setStartEnter({
                                ...startEnter,
                                osnovnost: parseFloat(e.target.value) || 0,
                                })
                            }
                            />
                        </div>
                        <div className="space-y-2">
                            <Label>FeO в агломерате (%)</Label>
                            <Input
                            type="number"
                            value={startEnter.feoInAgl}
                            onChange={(e) =>
                                setStartEnter({
                                ...startEnter,
                                feoInAgl: parseFloat(e.target.value) || 0,
                                })
                            }
                            />
                        </div>
                        <div className="space-y-2">
                            <Label>Доломит в агломерате (%)</Label>
                            <Input
                            type="number"
                            value={startEnter.dolomyteInAgl}
                            onChange={(e) =>
                                setStartEnter({
                                ...startEnter,
                                dolomyteInAgl: parseFloat(e.target.value) || 0,
                                })
                            }
                            />
                        </div>
                        </CardContent>
                    </Card>
                </TabsContent>

                {/* Tab 2: Fluxes */}
                <TabsContent value="flux" className="space-y-6">
                    <Card className="shadow-sm">
                        <CardHeader className="pb-3">
                        <CardTitle className="text-lg flex items-center gap-2">
                            <Beaker className="h-5 w-5" />
                            Флюсующие добавки
                        </CardTitle>
                        </CardHeader>
                        <CardContent>
                        <div className="grid grid-cols-1 gap-8">
                            {/* Limestone */}
                            <div className="space-y-3">
                            <div className="flex items-center gap-2 border-b pb-2">
                                <div className="h-3 w-3 rounded-full bg-slate-400" />
                                <Label className="font-semibold text-base">Известняк</Label>
                            </div>
                            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
                                <NumberInput
                                label="CaO"
                                value={flux.izvestnyakCaO}
                                onChange={(v) =>
                                    setFlux({ ...flux, izvestnyakCaO: v })
                                }
                                />
                                <NumberInput
                                label="SiO2"
                                value={flux.izvestnyakSiO2}
                                onChange={(v) =>
                                    setFlux({ ...flux, izvestnyakSiO2: v })
                                }
                                />
                                <NumberInput
                                label="Al2O3"
                                value={flux.izvestnyakAl2O3}
                                onChange={(v) =>
                                    setFlux({ ...flux, izvestnyakAl2O3: v })
                                }
                                />
                                <NumberInput
                                label="MgO"
                                value={flux.izvestnyakMgO}
                                onChange={(v) =>
                                    setFlux({ ...flux, izvestnyakMgO: v })
                                }
                                />
                                <NumberInput
                                label="ППП"
                                value={flux.izvestnyakPMPP}
                                onChange={(v) =>
                                    setFlux({ ...flux, izvestnyakPMPP: v })
                                }
                                />
                            </div>
                            </div>

                            {/* Dolomite */}
                            <div className="space-y-3">
                            <div className="flex items-center gap-2 border-b pb-2">
                                <div className="h-3 w-3 rounded-full bg-slate-600" />
                                <Label className="font-semibold text-base">Доломит</Label>
                            </div>
                            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
                                <NumberInput
                                label="CaO"
                                value={flux.dolomyteCaO}
                                onChange={(v) =>
                                    setFlux({ ...flux, dolomyteCaO: v })
                                }
                                />
                                <NumberInput
                                label="SiO2"
                                value={flux.dolomyteSiO2}
                                onChange={(v) =>
                                    setFlux({ ...flux, dolomyteSiO2: v })
                                }
                                />
                                <NumberInput
                                label="Al2O3"
                                value={flux.dolomyteAl2O3}
                                onChange={(v) =>
                                    setFlux({ ...flux, dolomyteAl2O3: v })
                                }
                                />
                                <NumberInput
                                label="MgO"
                                value={flux.dolomyteMgO}
                                onChange={(v) =>
                                    setFlux({ ...flux, dolomyteMgO: v })
                                }
                                />
                                <NumberInput
                                label="ППП"
                                value={flux.dolomytePMPP}
                                onChange={(v) =>
                                    setFlux({ ...flux, dolomytePMPP: v })
                                }
                                />
                            </div>
                            </div>
                        </div>
                        </CardContent>
                    </Card>
                </TabsContent>

                {/* Tab 3: Coke */}
                <TabsContent value="coke" className="space-y-6">
                    <Card className="shadow-sm">
                        <CardHeader className="pb-3">
                        <CardTitle className="text-lg flex items-center gap-2">
                            <Flame className="h-5 w-5" />
                            Коксовая мелочь
                        </CardTitle>
                        </CardHeader>
                        <CardContent className="space-y-8">
                        <div className="space-y-3">
                            <Label className="font-semibold text-base border-b pb-2 block">Характеристики</Label>
                            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
                            <NumberInput
                                label="Вес"
                                value={coke.weight}
                                onChange={(v) => setCoke({ ...coke, weight: v })}
                            />
                            <NumberInput
                                label="Зола (%)"
                                value={coke.percentZola}
                                onChange={(v) => setCoke({ ...coke, percentZola: v })}
                            />
                            <NumberInput
                                label="Сера (%)"
                                value={coke.percentSera}
                                onChange={(v) => setCoke({ ...coke, percentSera: v })}
                            />
                            <NumberInput
                                label="Летучие (%)"
                                value={coke.percentValotiles}
                                onChange={(v) => setCoke({ ...coke, percentValotiles: v })}
                            />
                            <NumberInput
                                label="C (%)"
                                value={coke.percentC}
                                onChange={(v) => setCoke({ ...coke, percentC: v })}
                            />
                            </div>
                        </div>

                        <div className="space-y-3">
                            <Label className="font-semibold text-base border-b pb-2 block">Состав золы</Label>
                            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-4">
                            <NumberInput
                                label="Fe"
                                value={zola.fe}
                                onChange={(v) => setZola({ ...zola, fe: v })}
                            />
                            <NumberInput
                                label="CaO"
                                value={zola.cao}
                                onChange={(v) => setZola({ ...zola, cao: v })}
                            />
                            <NumberInput
                                label="SiO2"
                                value={zola.sio2}
                                onChange={(v) => setZola({ ...zola, sio2: v })}
                            />
                            <NumberInput
                                label="Al2O3"
                                value={zola.al2o3}
                                onChange={(v) => setZola({ ...zola, al2o3: v })}
                            />
                            <NumberInput
                                label="MgO"
                                value={zola.mgo}
                                onChange={(v) => setZola({ ...zola, mgo: v })}
                            />
                            <NumberInput
                                label="P"
                                value={zola.p}
                                onChange={(v) => setZola({ ...zola, p: v })}
                            />
                            </div>
                        </div>
                        </CardContent>
                    </Card>
                </TabsContent>

                {/* Tab 4: Components */}
                <TabsContent value="components" className="space-y-6">
                    <div className="space-y-4">
                        <div className="flex items-center justify-between">
                        <h2 className="text-lg font-semibold flex items-center gap-2">
                            <Layers className="h-5 w-5 text-primary" />
                            Компоненты шихты
                        </h2>
                        <Button onClick={addComponent} size="sm" className="gap-2">
                            <Plus className="h-4 w-4" /> Добавить
                        </Button>
                        </div>

                        <ScrollArea className="h-[600px] pr-4">
                            <div className="grid gap-4">
                            {components.map((comp, index) => (
                                <Card
                                key={comp.id}
                                className="border-l-4 border-l-orange-500 shadow-sm overflow-hidden"
                                >
                                <Collapsible
                                    open={openComponents[comp.id]}
                                    onOpenChange={() => toggleComponent(comp.id)}
                                >
                                    <div className="flex items-center p-4 gap-4 bg-card hover:bg-accent/5 transition-colors">
                                    <CollapsibleTrigger asChild>
                                        <Button
                                        variant="ghost"
                                        size="sm"
                                        className="h-8 w-8 p-0"
                                        >
                                        {openComponents[comp.id] ? (
                                            <ChevronUp className="h-4 w-4" />
                                        ) : (
                                            <ChevronDown className="h-4 w-4" />
                                        )}
                                        </Button>
                                    </CollapsibleTrigger>
                                    
                                    <div className="flex-1 grid grid-cols-1 md:grid-cols-2 gap-4 items-center">
                                        <Input
                                        placeholder="Название компонента"
                                        value={comp.name}
                                        onChange={(e) =>
                                            updateComponent(comp.id, "name", e.target.value)
                                        }
                                        className="font-medium h-9"
                                        />
                                        <div className="flex gap-4 text-sm text-muted-foreground">
                                            <span>Вес: {comp.weight}</span>
                                            <span>Влажность: {comp.wet}%</span>
                                        </div>
                                    </div>
                                    
                                    <Button
                                        variant="ghost"
                                        size="icon"
                                        className="text-destructive hover:text-destructive hover:bg-destructive/10"
                                        onClick={() => removeComponent(comp.id)}
                                        disabled={components.length === 1}
                                    >
                                        <Trash2 className="h-4 w-4" />
                                    </Button>
                                    </div>

                                    <CollapsibleContent>
                                        <Separator />
                                    <div className="p-4 space-y-4">
                                        <div className="grid grid-cols-3 gap-4">
                                        <NumberInput
                                            label="Вес"
                                            value={comp.weight}
                                            onChange={(v) =>
                                            updateComponent(comp.id, "weight", v)
                                            }
                                        />
                                        <NumberInput
                                            label="Влажность (%)"
                                            value={comp.wet}
                                            onChange={(v) => updateComponent(comp.id, "wet", v)}
                                        />
                                        <NumberInput
                                            label="ППП"
                                            value={comp.pmpp}
                                            onChange={(v) =>
                                            updateComponent(comp.id, "pmpp", v)
                                            }
                                        />
                                        </div>

                                        <div>
                                        <Label className="text-xs font-semibold uppercase text-muted-foreground mb-3 block">
                                            Химический состав
                                        </Label>
                                        <div className="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-6 gap-3 bg-muted/30 p-3 rounded-lg border border-border/50">
                                            <NumberInput
                                            label="Fe"
                                            value={comp.fe}
                                            onChange={(v) =>
                                                updateComponent(comp.id, "fe", v)
                                            }
                                            />
                                            <NumberInput
                                            label="FeO"
                                            value={comp.feo}
                                            onChange={(v) =>
                                                updateComponent(comp.id, "feo", v)
                                            }
                                            />
                                            <NumberInput
                                            label="CaO"
                                            value={comp.cao}
                                            onChange={(v) =>
                                                updateComponent(comp.id, "cao", v)
                                            }
                                            />
                                            <NumberInput
                                            label="SiO2"
                                            value={comp.sio2}
                                            onChange={(v) =>
                                                updateComponent(comp.id, "sio2", v)
                                            }
                                            />
                                            <NumberInput
                                            label="MgO"
                                            value={comp.mgo}
                                            onChange={(v) =>
                                                updateComponent(comp.id, "mgo", v)
                                            }
                                            />
                                            <NumberInput
                                            label="Al2O3"
                                            value={comp.al2o3}
                                            onChange={(v) =>
                                                updateComponent(comp.id, "al2o3", v)
                                            }
                                            />
                                            <NumberInput
                                            label="TiO2"
                                            value={comp.tio2}
                                            onChange={(v) =>
                                                updateComponent(comp.id, "tio2", v)
                                            }
                                            />
                                            <NumberInput
                                            label="S"
                                            value={comp.s}
                                            onChange={(v) => updateComponent(comp.id, "s", v)}
                                            />
                                            <NumberInput
                                            label="P"
                                            value={comp.p}
                                            onChange={(v) => updateComponent(comp.id, "p", v)}
                                            />
                                            <NumberInput
                                            label="Cr"
                                            value={comp.cr}
                                            onChange={(v) => updateComponent(comp.id, "cr", v)}
                                            />
                                            <NumberInput
                                            label="Zn"
                                            value={comp.zn}
                                            onChange={(v) => updateComponent(comp.id, "zn", v)}
                                            />
                                            <NumberInput
                                            label="MnO"
                                            value={comp.mno}
                                            onChange={(v) =>
                                                updateComponent(comp.id, "mno", v)
                                            }
                                            />
                                        </div>
                                        </div>
                                    </div>
                                    </CollapsibleContent>
                                </Collapsible>
                                </Card>
                            ))}
                            </div>
                        </ScrollArea>
                        
                        <div className="pt-4 flex justify-end">
                            <Button
                            size="lg"
                            onClick={handleCalculate}
                            disabled={isCalculating}
                            className="shadow-lg hover:shadow-xl transition-all gap-2"
                            >
                            {isCalculating ? (
                                <>Загрузка...</>
                            ) : (
                                <>
                                <Calculator className="h-5 w-5" />
                                Рассчитать
                                </>
                            )}
                            </Button>
                        </div>
                    </div>
                </TabsContent>

                {/* Tab 5: Results */}
                <TabsContent value="results" className="space-y-6">
                    {calculationResults ? (
                        <div className="space-y-4">
                            <SinterChargeResults results={calculationResults} />
                            <div className="flex justify-end gap-3">
                                <Button
                                    variant="outline"
                                    onClick={() => setSaveDialogOpen(true)}
                                    className="gap-2"
                                >
                                    <Save className="h-4 w-4" />
                                    Сохранить результат
                                </Button>
                            </div>
                        </div>
                    ) : (
                        <div className="flex flex-col items-center justify-center py-12 text-muted-foreground border-2 border-dashed rounded-lg">
                            <Calculator className="h-12 w-12 mb-4 opacity-50" />
                            <p className="text-lg font-medium">Результаты отсутствуют</p>
                            <p>Заполните параметры и нажмите "Рассчитать"</p>
                        </div>
                    )}
                </TabsContent>

            </Tabs>
        </div>

        {/* History Sidebar */}
        <div className="space-y-6">
            <CalculationHistory
                history={history}
                onSelect={(item) => {
                    // In a real app, this would load the saved state into the form
                    console.log("Loading history item:", item);
                }}
                onDelete={removeFromHistory}
                onClear={clearHistory}
            />
        </div>
      </div>

      <SaveCalculationDialog
        open={saveDialogOpen}
        onOpenChange={setSaveDialogOpen}
        onSave={handleSaveToHistory}
      />
    </div>
  );
}


