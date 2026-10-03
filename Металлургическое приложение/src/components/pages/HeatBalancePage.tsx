import { useState } from "react";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  CardDescription,
} from "../ui/card";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import {
  Tabs,
  TabsList,
  TabsTrigger,
  TabsContent,
} from "../ui/tabs";
import { Flame } from "lucide-react";
import { CalculationHistory } from "../CalculationHistory";
import { useCalculationHistory } from "../../hooks/useCalculationHistory";
import { SaveCalculationDialog } from "../SaveCalculationDialog";
import { furnaceService } from "../../services/api.service";
import { HeatBalanceResults } from "./HeatBalanceResults";

interface FurnaceInputs {
  Si: number;
  Mn: number;
  S: number;
  P: number;
  Ti: number;
  Cr: number;
  V: number;
  C: number;

  T_iron: number;
  C_iron: number;

  rd: number;

  coke_rate: number;
  coke_ash: number;
  coke_sulfur: number;
  coke_volatiles: number;
  coke_moisture: number;

  hot_blast_temp: number;
  blast_humidity: number;
  oxygen_content: number;

  gas_consumption: number;
  gas_CH4: number;
  gas_C2H6: number;
  gas_CO2: number;
  gas_C_CH4: number;
  gas_H2_CH4: number;

  limestone_rate: number;
  limestone_moisture: number;
  limestone_loss_on_ignition: number;

  slag_rate: number;
  slag_sulfur: number;
  slag_heat_capacity: number;

  top_gas_temp: number;
  top_CO2: number;
  top_CO: number;
  top_H2: number;
  top_N2: number;

  ore_rate: number;
  pellets_rate: number;
  ore_moisture: number;
}

interface HeatBalanceResponseData {
  heat_balance: Record<string, number>;
}

export function HeatBalancePage() {
  const {
    history,
    addToHistory,
    removeFromHistory,
    clearHistory,
  } = useCalculationHistory("heat-balance");

  const [inputs, setInputs] = useState<FurnaceInputs>({
    Si: 0.53,
    Mn: 0.19,
    S: 0.014,
    P: 0.043,
    Ti: 0.068,
    Cr: 0.021,
    V: 0.0,
    C: 5.13,

    T_iron: 1405,
    C_iron: 0.9,

    rd: 0.35,

    coke_rate: 420,
    coke_ash: 11.9,
    coke_sulfur: 0.5,
    coke_volatiles: 0.6,
    coke_moisture: 4.2,

    hot_blast_temp: 1140,
    blast_humidity: 6.36,
    oxygen_content: 24.1,

    gas_consumption: 115,
    gas_CH4: 100,
    gas_C2H6: 0,
    gas_CO2: 0,
    gas_C_CH4: 1,
    gas_H2_CH4: 2,

    limestone_rate: 0,
    limestone_moisture: 0,
    limestone_loss_on_ignition: 42,

    slag_rate: 260,
    slag_sulfur: 1.03,
    slag_heat_capacity: 1.26,

    top_gas_temp: 328,
    top_CO2: 17.5,
    top_CO: 24.1,
    top_H2: 7.0,
    top_N2: 51.4,

    ore_rate: 1716,
    pellets_rate: 0,
    ore_moisture: 0,
  });

  const [calculationResults, setCalculationResults] =
    useState<HeatBalanceResponseData | null>(null);

  const [isCalculating, setIsCalculating] = useState(false);

  const [calculationError, setCalculationError] =
    useState("");

  const [saveDialogOpen, setSaveDialogOpen] = useState(false);

  const [activeTab, setActiveTab] = useState("inputs");

  const handleInputChange = (
    field: keyof FurnaceInputs,
    value: number,
  ) => {
    setInputs((previousInputs) => ({
      ...previousInputs,
      [field]: value,
    }));
  };

  const handleCalculate = async () => {
    setIsCalculating(true);
    setCalculationError("");
    setCalculationResults(null);

    try {
      const response =
        await furnaceService.calculate(inputs);

      setCalculationResults(response);

      // После успешного расчета
      // автоматически открываем результаты
      setActiveTab("results");
    } catch (error: any) {
      console.error(
        "Ошибка расчета теплового баланса:",
        error
      );

      setCalculationError(
        error?.message ||
          "Произошла ошибка при выполнении расчета."
      );
    } finally {
      setIsCalculating(false);
    }
  };

  const handleSaveToHistory = (note: string) => {
    if (!calculationResults) {
      return;
    }

    addToHistory(
      note,
      JSON.stringify(calculationResults)
    );
  };

  return (
    <div className="space-y-6">
      {/* Заголовок */}
      <div className="flex items-center gap-3 mb-2">
        <Flame className="size-8 text-orange-500" />

        <h1 className="text-3xl">
          Теплообмен в доменной печи
        </h1>
      </div>

      <p className="text-muted-foreground">
        Расчет теплового баланса и теплообменных процессов
      </p>

      <div className="grid gap-6 lg:grid-cols-[1fr_380px]">
        <div className="space-y-6">
          <Tabs
            value={activeTab}
            onValueChange={setActiveTab}
          >
            <TabsList className="grid w-full grid-cols-2">
              <TabsTrigger value="inputs">
                Входные данные
              </TabsTrigger>

              <TabsTrigger value="results">
                Результаты
              </TabsTrigger>
            </TabsList>

            {/* ========================================= */}
            {/* ВКЛАДКА ВХОДНЫХ ДАННЫХ */}
            {/* ========================================= */}

            <TabsContent
              value="inputs"
              className="space-y-6"
            >
              <Card>
                <CardHeader>
                  <CardTitle>
                    Форма ввода данных
                  </CardTitle>

                  <CardDescription>
                    Введите параметры для расчета
                    теплового баланса
                  </CardDescription>
                </CardHeader>

                <CardContent className="grid gap-4 md:grid-cols-2">
                  {Object.entries(inputs).map(
                    ([key, value]) => (
                      <div
                        key={key}
                        className="space-y-2"
                      >
                        <Label htmlFor={key}>
                          {key}
                        </Label>

                        <Input
                          id={key}
                          type="number"
                          step="any"
                          value={value}
                          onChange={(e) =>
                            handleInputChange(
                              key as keyof FurnaceInputs,
                              parseFloat(
                                e.target.value
                              ) || 0,
                            )
                          }
                        />
                      </div>
                    ),
                  )}
                </CardContent>
              </Card>

              {/* Ошибка */}
              {calculationError && (
                <Card className="border-destructive">
                  <CardContent className="pt-6">
                    <p className="text-destructive">
                      {calculationError}
                    </p>
                  </CardContent>
                </Card>
              )}

              {/* Кнопка расчета */}
              <div className="flex justify-end">
                <Button
                  size="lg"
                  onClick={handleCalculate}
                  disabled={isCalculating}
                >
                  {isCalculating
                    ? "Вычисляем..."
                    : "Выполнить расчет"}
                </Button>
              </div>
            </TabsContent>

            {/* ========================================= */}
            {/* ВКЛАДКА РЕЗУЛЬТАТОВ */}
            {/* ========================================= */}

            <TabsContent value="results">
              {calculationResults ? (
                <HeatBalanceResults
                  data={calculationResults}
                />
              ) : (
                <Card>
                  <CardHeader>
                    <CardTitle>
                      Результаты
                    </CardTitle>

                    <CardDescription>
                      Здесь будут отображены результаты
                      расчета теплового баланса
                    </CardDescription>
                  </CardHeader>

                  <CardContent className="min-h-[200px] flex items-center justify-center border-2 border-dashed border-border rounded-lg">
                    <p className="text-muted-foreground">
                      Результаты появятся здесь после
                      выполнения расчета
                    </p>
                  </CardContent>
                </Card>
              )}
            </TabsContent>
          </Tabs>
        </div>

        {/* ========================================= */}
        {/* ИСТОРИЯ РАСЧЕТОВ */}
        {/* ========================================= */}

        <div className="hidden lg:block">
          <CalculationHistory
            history={history}
            onRemove={(id) =>
              removeFromHistory(id)
            }
            onClear={clearHistory}
          />
        </div>
      </div>

      {/* ========================================= */}
      {/* СОХРАНЕНИЕ РАСЧЕТА */}
      {/* ========================================= */}

      <SaveCalculationDialog
        open={saveDialogOpen}
        onOpenChange={setSaveDialogOpen}
        onSave={handleSaveToHistory}
        resultsPreview={
          calculationResults
            ? JSON.stringify(
                calculationResults,
                null,
                2
              )
            : ""
        }
      />
    </div>
  );
}