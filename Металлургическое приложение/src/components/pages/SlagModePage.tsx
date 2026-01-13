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
  Droplets,
  Factory,
  Flame,
  Layers,
  AlertCircle,
  Loader2,
  BookmarkPlus,
} from "lucide-react";
import { Separator } from "../ui/separator";
import { ScrollArea } from "../ui/scroll-area";
import { Alert, AlertDescription } from "../ui/alert";
import { useCalculationHistory } from "../../hooks/useCalculationHistory";
import { CalculationHistory } from "../CalculationHistory";
import { SaveCalculationDialog } from "../SaveCalculationDialog";
import {
  SlagModeResults,
  SlagModeResponseData,
} from "./SlagModeResults";
import { SlagModeCharts } from "./SlagModeCharts";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../ui/select";
import { slagModeService } from "../../services/api.service";

// Типы данных на основе C# моделей
interface UserAuthData {
  userName: string;
  password: string;
}

interface InputCokeForCalcs {
  consumption: number;
  sulfur: number;
  ashAmount: number;
  ashCaOFraction: number;
  ashSiO2Fraction: number;
  ashAl2O3Fraction: number;
  ashMgOFraction: number;
}

interface InputCastIronForCalc {
  si: number;
  s: number;
  mn: number;
  c: number;
  ti: number;
  cr: number;
  temp: number;
}

interface InputSlagForCalc {
  caO: number;
  siO2: number;
  tiO2: number;
  al2O3: number;
  mgO: number;
}

interface InputChargeComponentsForCalc {
  sourcename: string;
  consumption: number;
  fe: number;
  siO2: number;
  al2O3: number;
  caO: number;
  mgO: number;
  s: number;
  mnO: number;
  tiO2: number;
}

interface RequestData {
  user: UserAuthData;
  coke: InputCokeForCalcs;
  iron: InputCastIronForCalc;
  slag: InputSlagForCalc;
  components: InputChargeComponentsForCalc[];
}

export function SlagModePage() {

  useEffect(() => {
        slagModeService.getPreset()
          .then(({data}) => {
            
            setCokeData(data.request.inputCoke);
            setCastIronData(data.request.castIron);
            setSlagData(data.request.slag);
            setChargeComponents(data.request.components);
          })
  }, []);

  // Состояния для параметров кокса
  const [cokeData, setCokeData] = useState<InputCokeForCalcs>({
    consumption: 0,
    sulfur: 0,
    ashAmount: 0,
    ashCaOFraction: 0,
    ashSiO2Fraction: 0,
    ashAl2O3Fraction: 0,
    ashMgOFraction: 0,
  });

  // Состояния для параметров чугуна
  const [castIronData, setCastIronData] =
    useState<InputCastIronForCalc>({
      si: 0,
      s: 0,
      mn: 0,
      c: 0,
      ti: 0,
      cr: 0,
      temp: 0,
    });

  // Состояния для параметров шлака
  const [slagData, setSlagData] = useState<InputSlagForCalc>({
    caO: 0,
    siO2: 0,
    tiO2: 0,
    al2O3: 0,
    mgO: 0,
  });

  // Состояния для компонентов шихты (динамический список)
  const [chargeComponents, setChargeComponents] = useState<
    InputChargeComponentsForCalc[]
  >([
    {
      sourcename: "",
      consumption: 0,
      fe: 0,
      siO2: 0,
      al2O3: 0,
      caO: 0,
      mgO: 0,
      s: 0,
      mnO: 0,
      tiO2: 0,
    },
  ]);

  // Состояния для результатов и UI
  const [calculationResults, setCalculationResults] =
    useState<SlagModeResponseData | null>(null);
  const [isCalculating, setIsCalculating] = useState(false);
  const [calculationError, setCalculationError] = useState("");
  const [activeTab, setActiveTab] = useState("components");

  // История расчетов
  const {
    history,
    addToHistory,
    removeFromHistory,
    clearHistory,
  } = useCalculationHistory("slag-mode");
  const [saveDialogOpen, setSaveDialogOpen] = useState(false);

  // Функции для работы с компонентами шихты
  const addChargeComponent = () => {
    setChargeComponents([
      ...chargeComponents,
      {
        sourcename: "",
        consumption: 0,
        fe: 0,
        siO2: 0,
        al2O3: 0,
        caO: 0,
        mgO: 0,
        s: 0,
        mnO: 0,
        tiO2: 0,
      },
    ]);
  };

  const removeChargeComponent = (index: number) => {
    setChargeComponents(
      chargeComponents.filter((_, i) => i !== index),
    );
  };

  const updateChargeComponent = (
    index: number,
    field: keyof InputChargeComponentsForCalc,
    value: string | number,
  ) => {
    const updated = [...chargeComponents];
    updated[index] = { ...updated[index], [field]: value };
    setChargeComponents(updated);
  };

  // Пустой метод расчета (вместо реального API вызова)
  const handleCalculate = async () => {
    setIsCalculating(true);
    setCalculationError("");
    setCalculationResults(null);

    try {
      // Формируем данные запроса
      const requestData: RequestData = {
        user: userData,
        coke: cokeData,
        iron: castIronData,
        slag: slagData,
        components: chargeComponents,
      };

      // Имитация задержки расчета
      await new Promise((resolve) => setTimeout(resolve, 1000));

      // Здесь будет вызов API для расчетов
      // const response = await slagModeService.calculate(requestData);
      // setCalculationResults(response.data);

      // Моковые данные для результатов (соответствующие модели ResponseData)
      const mockResults: SlagModeResponseData = {
        slagBasicity1: 1.125,
        slagBasicity2: 1.287,
        slagBasicity3: 0.965,
        slagBasicityKulikov: 1.456,
        slagOut: 512.5,
        materialCons: 1654.3,
        totalAglo: 78.5,
        propAglo23: 45.2,
        propAglo4: 33.3,
        propAglo234: 78.5,
        propSsgpo: 12.4,
        propLeb: 5.8,
        propKach: 2.1,
        propMix: 0.9,
        propOre: 0.3,
        propWeldSlag: 0.0,
        propBfAddict: 0.0,
        propMinInclude: 0.0,
        totalProp: 100.0,
        viscosity_1400: 2.456,
        viscosity_1450: 1.234,
        viscosity_1500: 0.678,
        viscosity_1550: 0.389,
        temp_7_Puaz: 1425.5,
        gradient_7_25: 0.0125,
        gradient_1400_1500: 0.0178,
        slagTemperature: 1462.0,
        slagTemperature_25Puaz: 1387.5,
        currSlagViscosity: 1.156,
        balSlagMass: 518.7,
        caOBalSlagMass: 515.2,
        totalSInOre: 2.145,
        sActivity: 0.8765,
        sDistribution: 125.4,
        sContentInCastIron: 0.0158,
        castIronTemp: 1485.0,
      };

      setCalculationResults(mockResults);
    } catch (error: any) {
      setCalculationError(
        error.message || "Произошла ошибка при расчете.",
      );
    } finally {
      setIsCalculating(false);
    }
  };

  const generateResultsSummary = (
    results: SlagModeResponseData,
  ): string => {
    if (!results) return "Нет данных";

    return `Основность (CaO/SiO₂): ${results.slagBasicity1?.toFixed(2) || "N/A"} | Выход шлака: ${results.slagOut?.toFixed(1) || "N/A"} кг/т | Вязкость при T: ${results.currSlagViscosity?.toFixed(3) || "N/A"} Па·с | S в чугуну: ${results.sContentInCastIron?.toFixed(4) || "N/A"}%`;
  };

  const handleSaveToHistory = (note: string) => {
    if (calculationResults) {
      const summary = generateResultsSummary(
        calculationResults,
      );
      addToHistory(note, summary);
    }
  };

  // Загрузка материалов шихты
  const [chargeMaterials, setChargeMaterials] = useState<ChargeMaterial[]>([]);
  const [loadingMaterials, setLoadingMaterials] = useState(true);

  useEffect(() => {
    const fetchChargeMaterials = async () => {
      try {
        setLoadingMaterials(true);
        const materials = await slagModeService.getComponents();
        
        setChargeMaterials(materials);
      } catch (error) {
        console.error("Ошибка при загрузке материалов шихты:", error);
      } finally {
        setLoadingMaterials(false);
      }
    };

    fetchChargeMaterials();
  }, []);

  // Функция для выбора материала из селектора
  const handleSelectChargeMaterial = (index: number, materialId: string) => {
    
    const selectedMaterial = chargeMaterials.find(m => m.sourcename === materialId);
    if (selectedMaterial) {
      const updated = [...chargeComponents];
      updated[index] = {
        sourcename: selectedMaterial.sourcename,
        consumption: selectedMaterial.consumption,
        fe: selectedMaterial.fe,
        siO2: selectedMaterial.siO2,
        al2O3: selectedMaterial.al2O3,
        caO: selectedMaterial.caO,
        mgO: selectedMaterial.mgO,
        s: selectedMaterial.s,
        mnO: selectedMaterial.mnO,
        tiO2: selectedMaterial.tiO2,
      };
      setChargeComponents(updated);
    }
  };

  return (
    <div className="space-y-6">
      <div>
        <div className="flex items-center gap-3 mb-2">
          <Droplets className="size-8 text-primary" />
          <h1 className="text-3xl">Шлаковый режим</h1>
        </div>
        <p className="text-muted-foreground">
          Расчет состава и свойств доменного шлака
        </p>
      </div>

      {calculationError && (
        <Alert variant="destructive">
          <AlertCircle className="size-4" />
          <AlertDescription>
            {calculationError}
          </AlertDescription>
        </Alert>
      )}

      <div className="grid gap-6 lg:grid-cols-[1fr_380px]">
        {/* Основной контент */}
        <div>
          <Tabs
            value={activeTab}
            onValueChange={setActiveTab}
            className="w-full"
          >
            <TabsList className="grid w-full grid-cols-4">
              <TabsTrigger value="components">
                Компоненты шихты
              </TabsTrigger>
              <TabsTrigger value="coke-iron">
                Кокс и чугун
              </TabsTrigger>
              <TabsTrigger value="slag">Шлак</TabsTrigger>
              <TabsTrigger value="results">
                Результаты
              </TabsTrigger>
            </TabsList>

            {/* Вкладка 1: Компоненты шихты */}
            <TabsContent
              value="components"
              className="space-y-6"
            >
              <Card>
                <CardHeader>
                  <CardTitle>Компоненты шихты</CardTitle>
                  <CardDescription>
                    Динамический список сырьевых материалов и их
                    характеристики
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <ScrollArea className="h-[600px] pr-4">
                    <div className="space-y-4">
                      {chargeComponents.map(
                        (component, index) => (
                          <div
                            key={index}
                            className="p-6 border border-border rounded-lg bg-card"
                          >
                            <div className="flex items-center justify-between mb-4">
                              <h3 className="font-semibold">
                                Компонент {index + 1}
                              </h3>
                              <Button
                                variant="destructive"
                                size="icon"
                                onClick={() =>
                                  removeChargeComponent(index)
                                }
                                disabled={
                                  chargeComponents.length === 1
                                }
                              >
                                <Trash2 className="size-4" />
                              </Button>
                            </div>

                            <div className="grid gap-4 md:grid-cols-2">
                              <div className="space-y-2 md:col-span-2">
                                <Label>
                                  Название источника
                                </Label>
                                <Select
                                  value={component.sourcename}
                                  onValueChange={(value) =>{

                                    
                                    handleSelectChargeMaterial(index, value)}
                                  }
                                >
                                  <SelectTrigger className="bg-accent/50">
                                    <SelectValue placeholder="Выберите источник">
                                      {component.sourcename || "Выберите источник"}
                                    </SelectValue>
                                  </SelectTrigger>
                                  <SelectContent>
                                    {loadingMaterials ? (
                                      <div className="p-2 text-center text-sm text-muted-foreground">
                                        Загрузка...
                                      </div>
                                    ) : (
                                      
                                      chargeMaterials.map(
                                        (material) => (
                                          <SelectItem
                                            key={material.sourcename}
                                            value={material.sourcename}
                                          >
                                            {material.sourcename}
                                          </SelectItem>
                                        ),
                                      )
                                    )}
                                  </SelectContent>
                                </Select>
                              </div>

                              <div className="space-y-2">
                                <Label>
                                  Расход, кг/т чугуна
                                </Label>
                                <Input
                                  type="number"
                                  value={component.consumption}
                                  readOnly
                                  className="bg-muted/50 cursor-not-allowed"
                                />
                              </div>

                              <div className="space-y-2">
                                <Label>Содержание Fe, %</Label>
                                <Input
                                  type="number"
                                  value={component.fe}
                                  readOnly
                                  className="bg-muted/50 cursor-not-allowed"
                                />
                              </div>

                              <div className="space-y-2">
                                <Label>
                                  Содержание SiO₂, %
                                </Label>
                                <Input
                                  type="number"
                                  value={component.siO2}
                                  readOnly
                                  className="bg-muted/50 cursor-not-allowed"
                                />
                              </div>

                              <div className="space-y-2">
                                <Label>
                                  Содержание Al₂O₃, %
                                </Label>
                                <Input
                                  type="number"
                                  value={component.al2O3}
                                  readOnly
                                  className="bg-muted/50 cursor-not-allowed"
                                />
                              </div>

                              <div className="space-y-2">
                                <Label>Содержание CaO, %</Label>
                                <Input
                                  type="number"
                                  value={component.caO}
                                  readOnly
                                  className="bg-muted/50 cursor-not-allowed"
                                />
                              </div>

                              <div className="space-y-2">
                                <Label>Содержание MgO, %</Label>
                                <Input
                                  type="number"
                                  value={component.mgO}
                                  readOnly
                                  className="bg-muted/50 cursor-not-allowed"
                                />
                              </div>

                              <div className="space-y-2">
                                <Label>Содержание S, %</Label>
                                <Input
                                  type="number"
                                  value={component.s}
                                  readOnly
                                  className="bg-muted/50 cursor-not-allowed"
                                />
                              </div>

                              <div className="space-y-2">
                                <Label>Содержание MnO, %</Label>
                                <Input
                                  type="number"
                                  value={component.mnO}
                                  readOnly
                                  className="bg-muted/50 cursor-not-allowed"
                                />
                              </div>

                              <div className="space-y-2">
                                <Label>
                                  Содержание TiO₂, %
                                </Label>
                                <Input
                                  type="number"
                                  value={component.tiO2}
                                  readOnly
                                  className="bg-muted/50 cursor-not-allowed"
                                />
                              </div>
                            </div>
                          </div>
                        ),
                      )}
                    </div>
                  </ScrollArea>

                  <Button
                    onClick={addChargeComponent}
                    variant="outline"
                    className="w-full"
                  >
                    <Plus className="size-4 mr-2" />
                    Добавить компонент шихты
                  </Button>
                </CardContent>
              </Card>
            </TabsContent>

            {/* Вкладка 2: Кокс и чугун */}
            <TabsContent
              value="coke-iron"
              className="space-y-6"
            >
              {/* Параметры кокса */}
              <Card>
                <CardHeader>
                  <CardTitle className="flex items-center gap-2">
                    <Flame className="size-5" />
                    Параметры кокса
                  </CardTitle>
                  <CardDescription>
                    Расход и химический состав кокса
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label>Расход кокса, кг/т чугуна</Label>
                      <Input
                        type="number"
                        value={cokeData.consumption}
                        onChange={(e) =>
                          setCokeData({
                            ...cokeData,
                            consumption:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Содержание серы (S), %</Label>
                      <Input
                        type="number"
                        value={cokeData.sulfur}
                        onChange={(e) =>
                          setCokeData({
                            ...cokeData,
                            sulfur:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Зольность, %</Label>
                      <Input
                        type="number"
                        value={cokeData.ashAmount}
                        onChange={(e) =>
                          setCokeData({
                            ...cokeData,
                            ashAmount:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Доля CaO в золе кокса, %</Label>
                      <Input
                        type="number"
                        value={cokeData.ashCaOFraction}
                        onChange={(e) =>
                          setCokeData({
                            ...cokeData,
                            ashCaOFraction:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Доля SiO₂ в золе кокса, %</Label>
                      <Input
                        type="number"
                        value={cokeData.ashSiO2Fraction}
                        onChange={(e) =>
                          setCokeData({
                            ...cokeData,
                            ashSiO2Fraction:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Доля Al₂O₃ в золе кокса, %</Label>
                      <Input
                        type="number"
                        value={cokeData.ashAl2O3Fraction}
                        onChange={(e) =>
                          setCokeData({
                            ...cokeData,
                            ashAl2O3Fraction:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Доля MgO в золе кокса, %</Label>
                      <Input
                        type="number"
                        value={cokeData.ashMgOFraction}
                        onChange={(e) =>
                          setCokeData({
                            ...cokeData,
                            ashMgOFraction:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>
                  </div>
                </CardContent>
              </Card>

              {/* Параметры чугуна */}
              <Card>
                <CardHeader>
                  <CardTitle className="flex items-center gap-2">
                    <Factory className="size-5" />
                    Параметры чугуна
                  </CardTitle>
                  <CardDescription>
                    Химический состав и температура чугуна
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label>Содержание Si, %</Label>
                      <Input
                        type="number"
                        value={castIronData.si}
                        onChange={(e) =>
                          setCastIronData({
                            ...castIronData,
                            si: parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Содержание S, %</Label>
                      <Input
                        type="number"
                        value={castIronData.s}
                        onChange={(e) =>
                          setCastIronData({
                            ...castIronData,
                            s: parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Содержание Mn, %</Label>
                      <Input
                        type="number"
                        value={castIronData.mn}
                        onChange={(e) =>
                          setCastIronData({
                            ...castIronData,
                            mn: parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Содержание C, %</Label>
                      <Input
                        type="number"
                        value={castIronData.c}
                        onChange={(e) =>
                          setCastIronData({
                            ...castIronData,
                            c: parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Содержание Ti, %</Label>
                      <Input
                        type="number"
                        value={castIronData.ti}
                        onChange={(e) =>
                          setCastIronData({
                            ...castIronData,
                            ti: parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Содержание Cr, %</Label>
                      <Input
                        type="number"
                        value={castIronData.cr}
                        onChange={(e) =>
                          setCastIronData({
                            ...castIronData,
                            cr: parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Температура чугуна, °C</Label>
                      <Input
                        type="number"
                        value={castIronData.temp}
                        onChange={(e) =>
                          setCastIronData({
                            ...castIronData,
                            temp:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>
                  </div>
                </CardContent>
              </Card>
            </TabsContent>

            {/* Вкладка 3: Шлак */}
            <TabsContent value="slag" className="space-y-6">
              <Card>
                <CardHeader>
                  <CardTitle className="flex items-center gap-2">
                    <Layers className="size-5" />
                    Состав шлака
                  </CardTitle>
                  <CardDescription>
                    Химический состав доменного шлака
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label>Содержание CaO, %</Label>
                      <Input
                        type="number"
                        value={slagData.caO}
                        onChange={(e) =>
                          setSlagData({
                            ...slagData,
                            caO:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Содержание SiO₂, %</Label>
                      <Input
                        type="number"
                        value={slagData.siO2}
                        onChange={(e) =>
                          setSlagData({
                            ...slagData,
                            siO2:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Содержание TiO₂, %</Label>
                      <Input
                        type="number"
                        value={slagData.tiO2}
                        onChange={(e) =>
                          setSlagData({
                            ...slagData,
                            tiO2:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Содержание Al₂O₃, %</Label>
                      <Input
                        type="number"
                        value={slagData.al2O3}
                        onChange={(e) =>
                          setSlagData({
                            ...slagData,
                            al2O3:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>

                    <div className="space-y-2">
                      <Label>Содержание MgO, %</Label>
                      <Input
                        type="number"
                        value={slagData.mgO}
                        onChange={(e) =>
                          setSlagData({
                            ...slagData,
                            mgO:
                              parseFloat(e.target.value) || 0,
                          })
                        }
                      />
                    </div>
                  </div>
                </CardContent>
              </Card>
            </TabsContent>

            {/* Вкладка 4: Результаты */}
            <TabsContent value="results" className="space-y-6">
              {!calculationResults && !isCalculating && (
                <Card>
                  <CardContent className="pt-6">
                    <div className="min-h-[400px] flex items-center justify-center border-2 border-dashed border-border rounded-lg">
                      <div className="text-center space-y-3">
                        <Droplets className="size-12 mx-auto text-muted-foreground" />
                        <p className="text-muted-foreground">
                          Выполните расчет, чтобы увидеть
                          результаты
                        </p>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              )}

              {isCalculating && (
                <Card>
                  <CardContent className="pt-6">
                    <div className="min-h-[400px] flex items-center justify-center">
                      <div className="text-center space-y-4">
                        <Loader2 className="size-12 mx-auto animate-spin text-primary" />
                        <p className="text-muted-foreground">
                          Выполняется расчет...
                        </p>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              )}

              {calculationResults && !isCalculating && (
                <div className="space-y-6">
                  <div className="flex items-center justify-between mb-4">
                    <div>
                      <h2 className="text-2xl font-semibold">
                        Результаты расчета
                      </h2>
                      <p className="text-muted-foreground">
                        Рассчитанные параметры шлакового режима
                      </p>
                    </div>
                    <Button
                      variant="outline"
                      size="sm"
                      onClick={() => setSaveDialogOpen(true)}
                    >
                      <BookmarkPlus className="size-4 mr-2" />
                      Сохранить
                    </Button>
                  </div>

                  <SlagModeResults data={calculationResults} />
                  <SlagModeCharts data={calculationResults} />
                </div>
              )}
            </TabsContent>
          </Tabs>

          <Separator className="my-6" />

          <div className="flex justify-end gap-4">
            <Button
              size="lg"
              onClick={handleCalculate}
              disabled={isCalculating}
            >
              {isCalculating ? (
                <>
                  <Loader2 className="size-4 mr-2 animate-spin" />
                  Расчет...
                </>
              ) : (
                "Выполнить расчет"
              )}
            </Button>
          </div>
        </div>

        {/* Правая колонка: История расчетов */}
        <div className="hidden lg:block">
          <CalculationHistory
            history={history}
            onRemove={removeFromHistory}
            onClear={clearHistory}
          />
        </div>
      </div>

      <SaveCalculationDialog
        open={saveDialogOpen}
        onOpenChange={setSaveDialogOpen}
        onSave={handleSaveToHistory}
        resultsPreview={
          calculationResults
            ? generateResultsSummary(calculationResults)
            : "Нет данных"
        }
      />
    </div>
  );
}