import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../ui/card";
import { Separator } from "../ui/separator";
import { Badge } from "../ui/badge";
import {
  Droplets,
  Flame,
  Thermometer,
  Activity,
  TrendingUp,
  Factory,
  Layers,
  BarChart3,
} from "lucide-react";

interface HeatBalanceResultsProps {
  data: HeatBalanceResponseData;
}

export function HeatBalanceResults({
  data,
}: HeatBalanceResultsProps) {
  return (
    <div className="space-y-6">
      {/* Тепло прихода */}
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
                  {data[key as keyof typeof data].toFixed(3)}
                </p>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>

      {/* Расход тепла */}
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
            {["C19", "C21", "C23", "C25"].map((key) => (
              <div
                key={key}
                className="p-4 border border-border rounded-lg bg-muted/30"
              >
                <p className="text-sm text-muted-foreground mb-1">
                  {RESULT_NAMES[key]}
                </p>
                <p className="text-2xl font-semibold">
                  {data[key as keyof typeof data].toFixed(3)}
                </p>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>

      {/* Тепло расплава и влаги */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Thermometer className="size-5 text-blue-500" />
            Тепло расплава и влаги
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            {["C27", "C29", "C31", "C33", "C35"].map((key) => (
              <div
                key={key}
                className="p-4 border border-border rounded-lg bg-muted/30"
              >
                <p className="text-sm text-muted-foreground mb-1">
                  {RESULT_NAMES[key]}
                </p>
                <p className="text-2xl font-semibold">
                  {data[key as keyof typeof data].toFixed(3)}
                </p>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>

      {/* Теплоемкость газов */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Droplets className="size-5 text-green-500" />
            Теплоемкость газов
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            {["C37", "C38", "C39", "C40", "C41"].map((key) => (
              <div
                key={key}
                className="p-4 border border-border rounded-lg bg-muted/30"
              >
                <p className="text-sm text-muted-foreground mb-1">
                  {RESULT_NAMES[key]}
                </p>
                <p className="text-2xl font-semibold">
                  {data[key as keyof typeof data].toFixed(3)}
                </p>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>

      {/* Остаточное тепло */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <TrendingUp className="size-5 text-teal-500" />
            Остаточное тепло
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            {["C42", "C43", "C44", "C45", "C46"].map((key) => (
              <div
                key={key}
                className="p-4 border border-border rounded-lg bg-muted/30"
              >
                <p className="text-sm text-muted-foreground mb-1">
                  {RESULT_NAMES[key]}
                </p>
                <p className="text-2xl font-semibold">
                  {data[key as keyof typeof data].toFixed(3)}
                </p>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>
    </div>
  );
}