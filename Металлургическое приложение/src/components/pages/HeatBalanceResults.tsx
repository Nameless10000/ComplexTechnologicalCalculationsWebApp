import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../ui/card";
import {
  Droplets,
  Flame,
  Thermometer,
  Activity,
  TrendingUp,
} from "lucide-react";

interface HeatBalanceResponseData {
  heat_balance: Record<string, number>;
}

interface HeatBalanceResultsProps {
  data: HeatBalanceResponseData;
}

// Пока используем обозначения C4, C5 и т.д.
// Позже сюда можно добавить нормальные русские названия параметров.
const RESULT_NAMES: Record<string, string> = {
  C4: "C4",
  C5: "C5",
  C6: "C6",
  C7: "C7",
  C8: "C8",
  C9: "C9",
  C10: "C10",
  C11: "C11",
  C12: "C12",
  C13: "C13",
  C14: "C14",
  C15: "C15",

  C19: "C19",
  C21: "C21",
  C23: "C23",
  C25: "C25",

  C27: "C27",
  C29: "C29",
  C31: "C31",
  C33: "C33",
  C35: "C35",

  C37: "C37",
  C38: "C38",
  C39: "C39",
  C40: "C40",
  C41: "C41",

  C42: "C42",
  C43: "C43",
  C44: "C44",
  C45: "C45",
  C46: "C46",
};

export function HeatBalanceResults({
  data,
}: HeatBalanceResultsProps) {
  const heatBalance = data.heat_balance;

  return (
    <div className="space-y-6">

      {/* ========================================= */}
      {/* ТЕПЛО ПРИХОДА */}
      {/* ========================================= */}

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Flame className="size-5 text-red-500" />

            Тепло прихода
          </CardTitle>

          <CardDescription>
            Тепло от различных источников прихода
          </CardDescription>
        </CardHeader>

        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            {[
              "C4",
              "C5",
              "C6",
              "C7",
              "C8",
              "C9",
              "C10",
              "C11",
              "C12",
              "C13",
              "C14",
              "C15",
            ].map((key) => (
              <div
                key={key}
                className="p-4 border border-border rounded-lg bg-muted/30"
              >
                <p className="text-sm text-muted-foreground mb-1">
                  {RESULT_NAMES[key]}
                </p>

                <p className="text-2xl font-semibold">
                  {heatBalance[key]?.toFixed(3) ?? "—"}
                </p>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>


      {/* ========================================= */}
      {/* РАСХОД ТЕПЛА */}
      {/* ========================================= */}

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Activity className="size-5 text-orange-500" />

            Расход тепла
          </CardTitle>

          <CardDescription>
            Расход тепла на процессы и влажность
          </CardDescription>
        </CardHeader>

        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            {[
              "C19",
              "C21",
              "C23",
              "C25",
            ].map((key) => (
              <div
                key={key}
                className="p-4 border border-border rounded-lg bg-muted/30"
              >
                <p className="text-sm text-muted-foreground mb-1">
                  {RESULT_NAMES[key]}
                </p>

                <p className="text-2xl font-semibold">
                  {heatBalance[key]?.toFixed(3) ?? "—"}
                </p>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>


      {/* ========================================= */}
      {/* ТЕПЛО РАСПЛАВА И ВЛАГИ */}
      {/* ========================================= */}

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Thermometer className="size-5 text-blue-500" />

            Тепло расплава и влаги
          </CardTitle>
        </CardHeader>

        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            {[
              "C27",
              "C29",
              "C31",
              "C33",
              "C35",
            ].map((key) => (
              <div
                key={key}
                className="p-4 border border-border rounded-lg bg-muted/30"
              >
                <p className="text-sm text-muted-foreground mb-1">
                  {RESULT_NAMES[key]}
                </p>

                <p className="text-2xl font-semibold">
                  {heatBalance[key]?.toFixed(3) ?? "—"}
                </p>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>


      {/* ========================================= */}
      {/* ТЕПЛОЕМКОСТЬ ГАЗОВ */}
      {/* ========================================= */}

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Droplets className="size-5 text-green-500" />

            Теплоемкость газов
          </CardTitle>
        </CardHeader>

        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            {[
              "C37",
              "C38",
              "C39",
              "C40",
              "C41",
            ].map((key) => (
              <div
                key={key}
                className="p-4 border border-border rounded-lg bg-muted/30"
              >
                <p className="text-sm text-muted-foreground mb-1">
                  {RESULT_NAMES[key]}
                </p>

                <p className="text-2xl font-semibold">
                  {heatBalance[key]?.toFixed(3) ?? "—"}
                </p>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>


      {/* ========================================= */}
      {/* ОСТАТОЧНОЕ ТЕПЛО */}
      {/* ========================================= */}

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <TrendingUp className="size-5 text-teal-500" />

            Остаточное тепло
          </CardTitle>
        </CardHeader>

        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            {[
              "C42",
              "C43",
              "C44",
              "C45",
              "C46",
            ].map((key) => (
              <div
                key={key}
                className="p-4 border border-border rounded-lg bg-muted/30"
              >
                <p className="text-sm text-muted-foreground mb-1">
                  {RESULT_NAMES[key]}
                </p>

                <p className="text-2xl font-semibold">
                  {heatBalance[key]?.toFixed(3) ?? "—"}
                </p>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>

    </div>
  );
}